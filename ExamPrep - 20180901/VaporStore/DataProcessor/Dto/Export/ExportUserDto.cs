namespace VaporStore.DataProcessor.Dto.Export
{
    using System.Xml.Serialization;

    [XmlType("User")]
    public class ExportUserDto
    {
        [XmlAttribute("username")]
        public string Username { get; set; }

        [XmlArray("Purchases")]
        public ExportPurchaseDto[] Purchases { get; set; }

        [XmlElement("TotalSpent")]
        public decimal TotalSpent { get; set; }
        //<User username="mgraveson">
        //<Purchases>
        //  <Purchase>
        //    <Card>7991 7779 5123 9211</Card>
        //    <Cvc>340</Cvc>
        //    <Date>2017-08-31 17:09</Date>
        //    <Game title = "Counter-Strike: Global Offensive" >
        //      < Genre > Action </ Genre >
        //      < Price > 12.49 </ Price >
        //    </ Game >
        //  </ Purchase >
        // ..
        //<TotalSpent>72.48</TotalSpent>
    }

    [XmlType("Purchase")]
    public class ExportPurchaseDto
    {
        [XmlElement("Card")]
        public string Card { get; set; }

        [XmlElement("Cvc")]
        public string Cvc { get; set; }

        [XmlElement("Date")]
        public string Date { get; set; }

        public ExportGameDto Game { get; set; }
    }

    [XmlType("Game")]
    public class ExportGameDto
    {
        [XmlAttribute("title")]
        public string Title { get; set; }

        [XmlElement("Genre")]
        public string Genre { get; set; }

        [XmlElement("Price")]
        public decimal Price { get; set; }
    }
}
