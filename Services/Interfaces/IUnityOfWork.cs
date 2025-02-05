namespace conecta_api.Services.Interfaces;
public interface IUnityOfWork
{
    IPropertyService PropertyService { get; }
    IAddressService AddressService { get; }
    Task Commit();
}
