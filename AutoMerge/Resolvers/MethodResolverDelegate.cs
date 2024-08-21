using System;
using System.Collections.Generic;

namespace AutoMerge.Resolvers
{
    public delegate IEnumerable<string> MethodResolverDelegate(
        IList<string> commonBase,  // פנאדלוםעû טסץתמהםמדמ פאיכא
        IList<string> left,         // פנאדלוםעû 1-דמ פאיכא סכוגא פאיכא
        IList<string> right);        // פנאדלוםעû 2-דמ פאיכא סןנאגא פאיכא
}