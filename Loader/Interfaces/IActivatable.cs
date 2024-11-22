using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabRat
{
    public interface IActivatable : IGameObject
    {
        public bool IsActivated { get; set; }
    }
}
