namespace PaperMore.Reports;

internal record DocumentReportData(string Title, long? ASN, string Correspondent, DateTimeOffset EntryDate, DateTimeOffset AddedDate);