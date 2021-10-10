using Casino.Crawler.Types;
using CasinoMVC.Data;
using CasinoMVC.Models;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CasinoMVC.Core
{
    public class ItemScraper
    {
        private const string SteamSearchLink = "https://steamcommunity.com/market/search?appid={0}#p{1}_popular_desc";
        private const string SteamSearchLinkWithDotaCategory = "https://steamcommunity.com/market/search?q=&category_570_Hero%5B%5D=any&category_570_Slot%5B%5D=any&category_570_Type%5B%5D=any&category_570_Rarity%5B%5D=tag_Rarity_{0}&appid=570#p{1}_popular_desc";

        private readonly HtmlWeb Web;

        public static readonly string[] BannedStartWords = { "Inscribed", "Exalted", "Autographed", "Corrupted", "Genuine", "Frozen", "Auspicious", "Unusual", "Cursed" };


        public static readonly string[] BannedEndWords = { "Bundle", "Treasure" };

        private const int DelayMs = 3100; 

        public class CrawlDiagnostics
        {
            public int AddedItemsCount { get; set; }

            public string Log { get; set; }

            public string ErrorMessage { get; set; }
        }

        private async Task<HtmlDocument> GetPage(string crawlUrl)//proxy?
        {
            return await Web.LoadFromWebAsync(crawlUrl);

            throw new InvalidOperationException("Could not load html from given source.");
        }

        public ItemScraper()
        {
            Web = new();
        }

        public async Task<CrawlDiagnostics> PopulateDatabaseAsync(ApplicationDbContext context, int amount, int startingPage = 1, DotaItemModel.ItemRarity rarity = DotaItemModel.ItemRarity.NONE)
        {
            var addedItems = new List<DotaItemModel>();
            var toRet = new CrawlDiagnostics();
            var logBuilder = new StringBuilder();
            for (int i = startingPage; i < amount+startingPage; i++)
            {
                var items = await GetItemsAsync(10, rarity, i);
                if(items.Count == 0)
                {
                    await context.SaveChangesAsync();
                    toRet.AddedItemsCount = addedItems.Count;
                    toRet.Log = logBuilder.ToString();
                    toRet.ErrorMessage = $"GET_ITEMS_FAIL Failed to get items at page {i}";
                    return toRet;
                }

                foreach (var item in items)
                {
                    if(item.Rarity == DotaItemModel.ItemRarity.NONE)
                    {
                        await context.SaveChangesAsync();
                        toRet.AddedItemsCount = addedItems.Count;
                        toRet.Log = logBuilder.ToString();
                        toRet.ErrorMessage = $"RARITY_FAIL Failed to get item at page {i}";
                        return toRet;
                    }
                    
                    var dbitem = await context.DotaItems.SingleOrDefaultAsync(p => p.Name == item.Name);
                    if (addedItems.Any(x => x.Name.Equals(item.Name)) || item.Price == 0f || dbitem !=null)
                    {
                        logBuilder.Append($"Rejected {item.Name} at page {i}<br> ");
                        continue;
                    }
                    context.DotaItems.Add(item);
                    addedItems.Add(item);
                    logBuilder.Append($"Added {item.Name} at page {i}<br> ");
                }
            }
            logBuilder.Append($"Successfully added {addedItems.Count()} items");
            await context.SaveChangesAsync();
            toRet.AddedItemsCount = addedItems.Count;
            toRet.Log = logBuilder.ToString();
            return toRet;
        }

        public async Task<List<DotaItemModel>> GetItemsAsync(int amount, DotaItemModel.ItemRarity rarity = DotaItemModel.ItemRarity.NONE, int startingPage = 1)
        {
            var items = new List<DotaItemModel>();
            while (items.Count < amount)
            {
                var htmlDocument = rarity == DotaItemModel.ItemRarity.NONE ? 
                    await GetPage(string.Format(SteamSearchLink, (int)GameType.Dota2, startingPage)) 
                    : await GetPage(string.Format(SteamSearchLinkWithDotaCategory, rarity ,startingPage));

                var itemNodes = htmlDocument.DocumentNode
                   .Descendants("a")
                   .Where(a => a.GetAttributeValue("id", "").StartsWith("result")
                       && a.GetAttributeValue("class", "").Equals("market_listing_row_link"));
                if (!itemNodes.Any())
                    return items;
                foreach (var itemNode in itemNodes)
                {
                    var item = await ParseItemNodeAsync(itemNode);
                    if (item is null)
                        continue;

                    items.Add(item);
                    if (items.Count >= amount)
                        return items;
                }
                startingPage++;
                await Task.Delay(DelayMs);
            }

            return items;
        }

        private async Task<DotaItemModel> ParseItemNodeAsync(HtmlNode node)
        {
            var nameNode = node.Descendants("span")
                        .Where(span => span.GetAttributeValue("class", "").Equals("market_listing_item_name"))
                        .FirstOrDefault();

            if (BannedStartWords.SingleOrDefault(word => nameNode.InnerText.StartsWith(word)) != null)
                return null;
            if (BannedEndWords.SingleOrDefault(word => nameNode.InnerText.EndsWith(word)) != null)
                return null;

            var item = new DotaItemModel();
            item.Name = nameNode?.InnerText;

            await Task.Delay(DelayMs);
            var link = node.GetAttributeValue("href", "");
            if (!string.IsNullOrEmpty(link))
            {
                var innerPage = await GetPage(link);
                var itemInfoNode = innerPage.DocumentNode
                    .Descendants("div")
                    .Where(div => div.GetAttributeValue("class", "").Equals("market_listing_iteminfo")).FirstOrDefault();

                

                var scriptNodes = innerPage.DocumentNode.Descendants("script")
                    .Where(script => script.GetAttributeValue("type", "").Equals(@"text/javascript"));
                foreach (var scriptNode in scriptNodes)
                {
                    if (TryGetRarity(scriptNode.InnerText, out var rarity))
                    {
                        item.Rarity = rarity;
                        break;
                    }
                }

            }
            var imgNode = node?.Descendants("img")
                    .Where(img => img.GetAttributeValue("class", "").Equals("market_listing_item_img")).FirstOrDefault();


            item.ImageUrl = CutImageUrl(imgNode?.GetAttributeValue("src",""));

            var priceNode = node.Descendants("span")
                .Where(span => span.GetAttributeValue("class", "").Equals("normal_price"))
                .FirstOrDefault();

            if (TryGetPrice(priceNode?.InnerText, out var itemPrice))
            {
                item.Price = itemPrice;
            }
            return item;
        }

        private string CutImageUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return string.Empty;
            int index = url.LastIndexOf("/");
            if (index >= 0)
                return url.Substring(0, index);
            else
                return string.Empty;
        }

        private bool TryGetRarity(string script, out DotaItemModel.ItemRarity rarity)
        {
            var textRarity = GetTextRarity(script);
            if(string.IsNullOrEmpty(textRarity))
            {
                rarity = DotaItemModel.ItemRarity.Common;
                return false;
            }
            var textChunks = textRarity.Split(" ");
            var rarityText = textChunks[0];
            var success = Enum.TryParse(rarityText, out rarity);
            return success;
        }

        private string GetTextRarity(string script)//use json?
        {
            int end = script.IndexOf("market_name");
            if (end == -1)
                return string.Empty;
            end -= 3;
            var start = end;
            while (script[start-- -2] != '"') ;
            var rarity = script.Substring(start, end - start);
            return rarity;
        }

        private bool TryGetPrice(string priceText, out float price)
        {
            var actualPriceText = priceText.Substring(1, priceText.Length - 5);//5 = " USD"+"$"
            try
            {
                price = float.Parse(actualPriceText, CultureInfo.InvariantCulture);
                return true;
            }
            catch(Exception e)
            {
                price = 0;
                return false;
            }
            
        }
    }
}
