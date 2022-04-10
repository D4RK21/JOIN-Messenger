using System.Threading.Tasks;
using Core;

namespace BLL.Abstractions.Interfaces
{
    public interface ICurrentUser
    {
        User User { get; set; }
    }
}
