using FileFormatter.Formatter.Abstractions;
using PuppeteerSharp;
using System;

namespace FileFormatter.Formatter;

public class TransformService : ITransformService
{
    public async Task<Stream> ConvertToPdf(string url)
    {
        var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync();

        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });

        await using var page = await browser.NewPageAsync();
        await page.GoToAsync(url);
        return await page.PdfStreamAsync(new PdfOptions()
        {
            DisplayHeaderFooter = true,
            PrintBackground = true
        });
    }
}
