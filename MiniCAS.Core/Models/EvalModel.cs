using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCAS.Core.Models
{
    public class EvalModel
    {
        public Expr.Expr Expr { get; set; }
        [Required]
        public string ExprStr { get; set; }
    }
}
