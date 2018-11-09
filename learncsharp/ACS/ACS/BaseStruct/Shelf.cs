

namespace ACS
{
    public class Shelf
    {
        /// <summary>
        /// 货架编号
        /// </summary>
        public string shelfNo;
        
        /// <summary>
        /// 货架区域
        /// </summary>
        public string areaNo;
        
        /// <summary>
        /// 货架码值
        /// </summary>
        public string barcode;
        
        /// <summary>
        /// 货架当前码值
        /// </summary>
        public string currentBarcode;
        
        /// <summary>
        /// 货架方向
        /// </summary>
        public string shelfDirection;

        /// <summary>
        /// 货架是否可用
        /// </summary>
        public bool isEnable;
        
        /// <summary>
        /// 货架是否被任务锁
        /// </summary>
        public bool isLocked;
    }
}
