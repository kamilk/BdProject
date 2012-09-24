using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ReferenceArchiver.Model;

namespace ReferenceArchiver.Invenio
{
    /// <summary>
    /// Class used to operate on xml files
    /// </summary>
    class Xml
    {
        public XmlDocument xml;
        private XmlNode record;

        public Xml()
        {
            xml = new XmlDocument();
            XmlNode collection = xml.CreateElement("collection");
            xml.AppendChild(collection);
            record = xml.CreateElement("record");
            collection.AppendChild(record);
        }

        public void SaveXml(string path)
        {
            xml.Save(path);
        }

        public void AddAuthor(Author author)
        {
            XmlNode node = this.CreateDatafield("700");
            XmlNode name = this.CreateSubfield(author.ToString(), "a");
            XmlNode nationality = this.CreateSubfield(author.Nationality, "l");
            node.AppendChild(name);
            node.AppendChild(nationality);
            record.AppendChild(node);
        }

        public void AddArticle(Article article)
        {
            XmlNode node = this.CreateDatafield("245");
            XmlNode title = this.CreateSubfield(article.TitlePl, "a");
            XmlNode titleeng = this.CreateSubfield(article.Title, "c");
            XmlNode date = this.CreateSubfield(article.Time.ToString(), "g");
            XmlNode id = this.CreateControlfield(article.Id.ToString(), "001");
            XmlNode page = this.CreateSubfield(article.PageBegin + "-" + article.PageEnd, "n");
            record.AppendChild(id);
            node.AppendChild(title);
            node.AppendChild(titleeng);
            node.AppendChild(date);
            node.AppendChild(page);
            record.AppendChild(node);
        }

        public void AddBiblioArticle(Article article)
        {
            XmlNode node = this.CreateDatafield("999");
            XmlNode title = this.CreateSubfield(article.TitlePl, "a");
            XmlNode titleeng = this.CreateSubfield(article.Title, "c");
            XmlNode date = this.CreateSubfield(article.Time.ToString(), "g");
            node.AppendChild(title);
            node.AppendChild(titleeng);
            node.AppendChild(date);
            record.AppendChild(node);
        }

        public void AddIssue(Issue issue)
        {
            XmlNode node = this.CreateDatafield("830");
            XmlNode title = this.CreateSubfield(issue.Title, "a");
            XmlNode publishernumber = this.CreateSubfield(issue.NumberWithinPublisher.ToString(), "v");
            XmlNode issuenumber = this.CreateSubfield(issue.NumberWithinJournal.ToString(), "n");
            XmlNode date = this.CreateSubfield(issue.YearOfPublication.ToString(), "f");
            node.AppendChild(title);
            node.AppendChild(publishernumber);
            node.AppendChild(issuenumber);
            node.AppendChild(date);
            record.AppendChild(node);
        }

        public void AddResearchJournal(ResearchJournal journal)
        {
            XmlNode node = this.CreateDatafield("901");
            XmlNode title = this.CreateSubfield(journal.Title, "a");
            XmlNode issn = this.CreateSubfield(journal.ISSN, "b");
            node.AppendChild(title);
            node.AppendChild(issn);
            record.AppendChild(node);
        }

        public void AddPublisher(Publisher publisher)
        {
            XmlNode node = this.CreateDatafield("902");
            XmlNode title = this.CreateSubfield(publisher.Title, "a");
            node.AppendChild(title);
            record.AppendChild(node);
        }

        public void AddInstitution(Institution institution)
        {
            XmlNode node = this.CreateDatafield("903");
            XmlNode title = this.CreateSubfield(institution.Name, "a");
            node.AppendChild(title);
            record.AppendChild(node);
        }

        private XmlNode CreateDatafield(string tag)
        {
            XmlNode node = xml.CreateElement("datafield");
            XmlAttribute typ = this.CreateAtribute("tag", tag);
            node.Attributes.Append(typ);
            return node;
        }

        private XmlNode CreateSubfield(string value, string code)
        {
            XmlNode node = xml.CreateElement("subfield");
            node.InnerText = value;
            XmlAttribute typ = this.CreateAtribute("code", code);
            node.Attributes.Append(typ);
            return node;
        }

        private XmlNode CreateControlfield(string value, string tag)
        {
            XmlNode node = xml.CreateElement("controlfield");
            node.InnerText = value;
            XmlAttribute typ = this.CreateAtribute("tag", tag);
            node.Attributes.Append(typ);
            return node;
        }

        private XmlAttribute CreateAtribute(string name, string value)
        {
            XmlAttribute attribute = xml.CreateAttribute(name);
            attribute.Value = value;
            return attribute;
        }
    }
}