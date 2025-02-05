using conecta_api.context;
using conecta_api.Services.Interfaces;

namespace conecta_api.Services;
public class UnityOfWork : IUnityOfWork
{
   private IPropertyService _propertyService;
    private IAddressService _addressService;

    public AppDbContext _context;

    public UnityOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IPropertyService PropertyService
    {
        get
        {
            return _propertyService ??= new PropertyService(_context, this);
        }
    }

    public IAddressService AddressService
    {
        get
        {
            return _addressService ??= new AddressService(_context, this);
        }
    }

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
