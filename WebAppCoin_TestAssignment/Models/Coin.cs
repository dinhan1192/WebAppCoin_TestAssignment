using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAppCoin_TestAssignment.Models
{
    public class Coin
    {
        [Key]
        public string Code { get; set; }
        [Required]
        [RegularExpression(@"^[A-Z]+$", ErrorMessage = "BaseAsset need to be A-Z")]
        public string BaseAsset { get; set; }
        [Required]
        [RegularExpression(@"^[A-Z]+$", ErrorMessage = "QuoteAsset need to be A-Z")]
        public string QuoteAsset { get; set; }
        [Required]
        [RegularExpression(@"^\d+.?\d{0,2}$", ErrorMessage = "Invalid Target Price; Maximum Two Decimal Points.")]
        [DisplayFormat(DataFormatString = "{0:N}")]
        public decimal LastPrice { get; set; }
        [Required]
        public int Volumn24h { get; set; }
        [ForeignKey("Market")]
        public int? MarketId { get; set; }
        public virtual Market Market { get; set; }
        public CoinStatus Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public enum CoinStatus
        {
            NotDeleted = 0, Deleted = -1
        }

        internal bool IsDeleted()
        {
            return this.Status == CoinStatus.Deleted;
        }
    }
}