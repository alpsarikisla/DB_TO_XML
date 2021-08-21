using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace NorthwindXml
{
    class Program
    {

        static void Main(string[] args)
        {
            XMLDosyaOlustur("Urunler.xml");
            DBToXml();
            XMLRead();
        }
        static void XMLDosyaOlustur(string dosyaYolu)
        {
            FileInfo fi = new FileInfo(dosyaYolu);
            if (!fi.Exists)
            {
                XmlWriter xmlYazici = XmlWriter.Create(dosyaYolu);

                xmlYazici.WriteStartDocument();
                xmlYazici.WriteStartElement("Products");
                xmlYazici.WriteEndElement();
               
                xmlYazici.WriteEndDocument();
                xmlYazici.Close();
                XDocument xdoc = XDocument.Load(dosyaYolu);
                XElement root = xdoc.Root;
                root.Add(new XAttribute("version", "155"));
                xdoc.Save(dosyaYolu);
            }
        }

        static void DBToXml()
        {
            NORTHWNDEntities db = new NORTHWNDEntities();
            try
            {
                XDocument xdoc = XDocument.Load("Urunler.xml");
                XElement root = xdoc.Root;

                foreach (Products item in db.Products.ToList())
                {
                    XElement yeniUrun = new XElement("Product");
                    XAttribute IdAttribute = new XAttribute("id", item.ProductID);
                    XElement IsimElement = new XElement("ProductName", item.ProductName);
                    XElement KategoriElement = new XElement("CategoryName", item.CategoryID != null ? item.Categories.CategoryName : "");
                    XElement TedarikciElement = new XElement("SupplierName", item.SupplierID != null ? item.Suppliers.CompanyName:"");
                    XElement FiyatElement = new XElement("UnitPrice", item.UnitPrice);
                    yeniUrun.Add(IdAttribute, IsimElement, KategoriElement, TedarikciElement, FiyatElement);
                    root.Add(yeniUrun);
                }
                xdoc.Save("Urunler.xml");
                Console.WriteLine("Tamamlandı");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void XMLRead()
        {
            NORTHWNDEntities db = new NORTHWNDEntities();
            try
            {
                List<Products> dburunler = db.Products.ToList();

                XDocument xdoc = XDocument.Load("Urunler.xml");
                XElement root = xdoc.Root;

                string id, isim, Kategori, Tedarikci, UnitPrice;

                foreach (XElement item in root.Elements())
                {
                    id = item.Attribute("id").Value;
                    isim = item.Element("ProductName").Value;
                    Kategori = item.Element("CategoryName").Value;
                    Tedarikci = item.Element("SupplierName").Value;
                    UnitPrice = item.Element("UnitPrice").Value;
                    Console.WriteLine($"-----{id}------");
                    Console.WriteLine($"isim=\t\t{isim}\nKategori=\t{Kategori}\nTedarikçi=\t{Tedarikci}\nFiyat=\t\t{UnitPrice} TL");
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

            }
        }
    }
}
