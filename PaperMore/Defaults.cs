using PaperMore.CLI;
using PaperMore.Reports;

namespace PaperMore;

public static class Defaults
{
    public static Func<DocumentReportData, bool> DefaultFilter(CmdArgs args)
    {
        bool isAsnLimited = args.AsnRangeFrom is not null || args.AsnRangeTo is not null;

        if (!isAsnLimited)
            return doc => true;

        return doc => doc.ASN is not null && LongBetween(doc.ASN ?? 0, args.AsnRangeFrom, args.AsnRangeTo);
    }

    private static bool LongBetween(long num, long? lower, long? upper)
    {
        long low = lower ?? 0L;
        long up = upper ?? long.MaxValue;

        return low <= num && num <= up;
    }


    public static Comparison<DocumentReportData> DefaultSorting = (lhs, rhs) =>
    {
        int comparison = String.Compare(lhs.Correspondent, rhs.Correspondent,
            StringComparison.CurrentCultureIgnoreCase);

        if (comparison == 0)
            comparison = lhs.DocumentDate.CompareTo(rhs.DocumentDate);

        if (comparison == 0)
            comparison = String.Compare(lhs.Title, rhs.Title, StringComparison.CurrentCultureIgnoreCase);

        return comparison;
    };
}