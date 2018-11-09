using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEICHUANG
{
    class Material
    {
        //料号
        public string CPN { get; set; }

        //长（mm）
        public int Length { get; set; }

        //宽（mm）
        public int Width { get; set; }

        //高（mm）
        public int Height { get; set; }

        //单箱数量
        public int QuantityPerBox { get; set; }

        //单箱重量(KG)
        public string WeightPerBox { get; set; }

        //满栈板箱数
        public int BoxCountPerPallet { get; set; }

        //关务备案品名
        public string Product { get; set; }

        //Category（大件L，小件S）
        public string Category { get; set; }

        //Area Evaluate
        public string AreaEvaluate { get; set; }

        public int flag { get; set; }
    }
}
