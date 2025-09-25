using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;

namespace ValSign
{
    public class ValidacionEstructura
    {
        XmlTextReader xtrReader = null;
        XmlReaderSettings settings = null;
        MemoryStream MR = null;
        public string msj { get; set; }
        public string msjT { get; set; }
        private Boolean rpt = true;

        public ValidacionEstructura()
        {
            settings = new XmlReaderSettings();
        }

        public void agregarSchemas(byte[] data)
        {
             MR= new MemoryStream(data);
            XmlSchema Schema = new XmlSchema();
            Schema = XmlSchema.Read(MR, new ValidationEventHandler(ValidationCallBack));
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas.Add(Schema);
        }
        public void agregarSchemas(string ruta)
        {
            StreamReader SR = new StreamReader(ruta);
            XmlSchema Schema = new XmlSchema();
            Schema = XmlSchema.Read(SR, new ValidationEventHandler(ValidationCallBack));
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas.Add(Schema);
        }
        public Boolean Validar(XmlTextReader reader)
        {
            rpt = true;
            xtrReader = reader;
            try
            {
                settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
                settings.ValidationType = ValidationType.Schema;
                //Create the schema validating reader.
                XmlReader vreader = XmlReader.Create(xtrReader, settings);
                while (vreader.Read()) { }
                vreader.Close();
                msj += "Estructura Válida\r\n";
                //return true;
            }
            catch (Exception e)
            {
                msjT = "Error al validar el XML:" + e.ToString();
                rpt = false;
            }
            return rpt;
        }

        private void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
            {
                msj += Environment.NewLine + "ATENCION: Esquema no Encontrado. No se pudo Validar.";
                msj += Environment.NewLine + "Linea: " + xtrReader.LineNumber + " - Posición: " + xtrReader.LinePosition + " - " + args.Message;
                rpt = false;
            }
            else
            {
                msj += Environment.NewLine+"ERROR AL VALIDAR: ";
                msj += Environment.NewLine + "Linea: " + xtrReader.LineNumber + " - Posición: " + xtrReader.LinePosition + " - " + args.Message;
                rpt = false;
            }
        }
    }
}