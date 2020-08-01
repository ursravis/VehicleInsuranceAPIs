using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetadataService.Infrastructure.Helpers
{
    public static class ExtentionHelpers
    {
        public static bool EFLike(this string ColumnName,string match)
        {
            return EF.Functions.Like(ColumnName, "%" + match + "%");
        }
    }
}
