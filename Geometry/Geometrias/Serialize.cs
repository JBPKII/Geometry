using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

using Geometry.Geometrias;

namespace Geometry.Geometrias.Serialize
{
    public class Serialize
    {
        public static void SerializaPuntos3D(string rutaXml, List<Punto3D> lstPuntos3D)
        {
            XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(lstPuntos3D.GetType(), "Geometry");
            using (TextWriter xmlWriter = new StreamWriter(rutaXml))
            {
                xmlSerializer.Serialize(xmlWriter, lstPuntos3D);
                xmlWriter.Close();
            }
        }

        public static List<Punto3D> DesSerializaPuntos3D(string rutaXml)
        {
            List<Punto3D> Res = new List<Punto3D>();

            XmlSerializer xmlUnserializer = new XmlSerializer(Res.GetType(), "Geometry");

            xmlUnserializer.UnknownNode += new XmlNodeEventHandler(Unserializer_UnknownNode);
            xmlUnserializer.UnknownAttribute += new XmlAttributeEventHandler(Unserializer_UnknownAttribute);

            using (FileStream xmlReader = new FileStream(rutaXml, FileMode.Open))
            {
                Res = (List<Punto3D>)xmlUnserializer.Deserialize(xmlReader);

                xmlReader.Close();
            }

            return Res;
        }

        private static void Unserializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        private static void Unserializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            Console.WriteLine("Unknown attribute " +
            attr.Name + "='" + attr.Value + "'");
        }
    }
}
