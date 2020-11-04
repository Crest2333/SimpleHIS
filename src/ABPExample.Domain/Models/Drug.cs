using ABPExample.Domain.Public;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Models
{
    public class Drug:FullEntity
    {
        /// <summary>
        /// 药品名称
        /// </summary>
        public string Name { get; set; } //药品名称

        /// <summary>
        /// 药品类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 药品包装单位
        /// </summary>
        public string PackUnit { get; set; }

        /// <summary>
        /// 药品规格
        /// </summary>
        public string Spec { get; set; }

        /// <summary>
        /// 药品使用单位
        /// </summary>
        public string Usage { get; set; }

        /// <summary>
        /// 药品使用量
        /// </summary>
        public int UsageNum { get; set; }

        /// <summary>
        /// 药品含量
        /// </summary>
        public decimal Content { get; set; }

        /// <summary>
        /// 药品最小单位
        /// </summary>
        public string SmallestUnit { get; set; }

        /// <summary>
        /// 药品剂型
        /// </summary>
        public string Dosage { get; set; }

        /// <summary>
        /// 药品进价
        /// </summary>
        public decimal InsertPrice { get; set; }

        /// <summary>
        /// 药品售价
        /// </summary>
        public decimal SellPrice { get; set; }

        /// <summary>
        /// 药品生产厂家
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// 药品有效日期
        /// </summary>
        public string EffortTime { get; set; }
    }
}
