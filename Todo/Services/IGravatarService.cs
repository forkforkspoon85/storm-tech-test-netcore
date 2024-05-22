using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Models.Gravatar;

namespace Todo.Services
{
    public interface IGravatarService
    {
        Task<IEnumerable<GravatarProfile>> GetProfiles(IEnumerable<string> emailAddresses);
        Task<GravatarProfile> GetProfile(string emailAddress);
    }
}
