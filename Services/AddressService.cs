using conecta_api.context;
using conecta_api.Services.Interfaces;

namespace conecta_api.Services;
public class AddressService : IAddressService
{
    private readonly AppDbContext _context;
    private readonly IUnityOfWork _uof;

    public AddressService(AppDbContext context, IUnityOfWork uof)
    {
        _context = context;
        _uof = uof;
    }
}
