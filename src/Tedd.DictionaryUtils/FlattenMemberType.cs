using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tedd
{
    [Flags]
    public enum FlattenMemberType
    {
        Property = 0b01,
        Field = 0b10,
        PropertyAndField = Property | Field
    }

}
