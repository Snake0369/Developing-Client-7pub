using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Core.Extensions;

namespace Trading.Domain.Contracts.Extensions
{
    internal static class LinqExtensions
    {
        public static Subposition? FirstSubposition(this List<Subposition> source, string baseAsset)
        {
            var subpositions = source.Where(s => s.BaseAsset.IsAssetEqual(baseAsset)).ToList();
            if (!subpositions.Any())
            {
                return null;
            }
            return subpositions.First();
        }

        public static Subposition? LastSubposition(this List<Subposition> source, string baseAsset)
        {
            var subpositions = source.Where(s => s.BaseAsset.IsAssetEqual(baseAsset)).ToList();
            if (!subpositions.Any())
            {
                return null;                
            }
            return subpositions.Last();
        }
    }
}
