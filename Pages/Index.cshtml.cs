using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml;

namespace XMLParser.Pages
{
    public class IndexModel : PageModel
    {
        public List<ItemProperties> ItemsProperties { get; set; } = new List<ItemProperties>();

        public async Task OnGet()
        {
            using (HttpClient client = new HttpClient())
            {
                string xmlUrl = "http://scripting.com/rss.xml";
                using (Stream xmlStream = await client.GetStreamAsync(xmlUrl))
                {
                    using (XmlReader xmlReader = XmlReader.Create(xmlStream))
                    {
                        while (xmlReader.Read())
                        {
                            if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "item")
                            {
                                ItemProperties item = new ItemProperties();
                                while (xmlReader.Read())
                                {
                                    if (xmlReader.NodeType == XmlNodeType.Element)
                                    {
                                        switch (xmlReader.Name)
                                        {
                                            case "title":
                                                item.Title = xmlReader.ReadElementContentAsString();
                                                break;
                                            case "description":
                                                item.Description = xmlReader.ReadElementContentAsString();
                                                break;
                                            case "pubDate":
                                                item.PubDate = xmlReader.ReadElementContentAsString();
                                                break;
                                            case "link":
                                                item.Link = xmlReader.ReadElementContentAsString();
                                                break;
                                            case "guid":
                                                item.Guid = xmlReader.ReadElementContentAsString();
                                                break;
                                        }
                                    }
                                    else if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == "item")
                                    {
                                        ItemsProperties.Add(item);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

public class ItemProperties
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string PubDate { get; set; }
    public string Link { get; set; }
    public string Guid { get; set; }
}