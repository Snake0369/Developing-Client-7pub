using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Domain.Services
{
    public class GeneratorClientOrders
    {
        private static GeneratorClientOrders? _generator = null;
        private Random _rnd;

        private GeneratorClientOrders(Random rnd)
        {
            _rnd = rnd;
        }

        public static GeneratorClientOrders GetGenerator()
        {
            if (_generator == null)
            {
                _generator = new GeneratorClientOrders(new Random(DateTimeOffset.Now.Second));
            }
            return _generator;
        }

        public string GetClientOrder() => Guid.NewGuid().ToString();
    }
}
