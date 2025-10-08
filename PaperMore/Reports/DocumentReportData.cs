namespace PaperMore.Reports;

public record DocumentReportData(string Title, long? ASN, string Correspondent, DateTimeOffset DocumentDate, DateTimeOffset AddedDate);