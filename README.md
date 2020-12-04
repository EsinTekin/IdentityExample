# IdentityExample



# ASP.NET Core Identity ile Üyelik Sistemi 

> Identity, sistem kullanýcýlarýna üyelik oluþturma, kimliklerini doðrulama ve yetkilendirme saðlayan bir sistemdir. Kullanýcýlar identity yapýsý sayesinde bilgileri ile bir hesap oluþturabilir ve oluþturulan hesaplar genellikle  SQL Server olmak üzere  PostgreSQL, MySQL, Azure Table Storage gibi veri tabanlarý üzerinde de depolanabilir.
> Kýsaca identity için kullanýcý arayüzü (UI) giriþ iþlevini destekleyen bir API denebilir.
> .NET Core Identity Scaffolding bu üyelik sistemi için hem arayüz hem de kayýt, giriþ gibi kýsýmlar için gerekli kod bloklarýný [Razor Pages](https://docs.microsoft.com/tr-tr/aspnet/core/razor-pages/?view=aspnetcore-3.0) ile oluþturur.

### Bu örnek projede de **Identity** yapýsý kullanýlarak kimlik doðrulamasýna sahip bir web uygulamasý geliþtirilmiþtir.  
##### Dokümanýn devamýnda örnek adým adým anlatýlacaktýr. 
> Öncelikle Visual Studio ortamýnda yeni bir ASP.NET Core Web Application (C#) projesi oluþturalým. Karþýmýza gelen ekranda "No Authentication" seçeneðini **"Individual User Accounts"** þeklinde deðiþtiriyor ve böylece projemizi oluþturuyoruz. 
![Create Application](https://m2c6a4u3.stackpathcdn.com/wp-content/uploads/2020/03/creating-aspnet-core-mvc-with-identity-selected.jpg )

> Identity sayfalarýnýn tutulduðu **Areas** ve  hazýr DBContext ve Migration ýn yerleþtiði **Data** dosyalarý bu yapýyla birlikte MVC projesi içerisine gelmiþtir.  
![](https://i0.wp.com/tutexchange.com/wp-content/uploads/2020/01/image-135.png?resize=311%2C339&ssl=1)

> Bunlarla birlikte oluþan app.setting.json dosyasýnda otomatik olarak bir Connection String oluþturulmuþtur. 

    {    
    {  "ConnectionStrings": {  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=aspnet-IEExample-4FC24192-AEFC-4273-80AF-A4C1AD922062;Trusted_Connection=True;MultipleActiveResultSets=true"
      }

> Startup.cs içerisinde Sql Server kullanýlarak veritabaný ayarlarý ConfigureServices metotu içerisinde tanýmlanmýþtýr.

   
     public void ConfigureServices(IServiceCollection services)
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        Configuration.GetConnectionString("DefaultConnection")));
                services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ApplicationDbContext>();
                services.AddControllersWithViews();
                services.AddRazorPages();
            }

> Buradaki AddDefaultIdentity IdentityUser sýnýfý ile identity ayarlarýný servise kaydetmeye yarar.
> Zaten otomatik olarak oluþturulmuþ bu yapýlarýn yanýnda yapýlmasý gereken iþlem migration ve ayarlarý veritabanýna yansýtmak olacaktýr. Bunun için Package Manager Console üzerinde **update-database** iþlemi uygulanýr. Böylece veritabaný ayarlarýyla birlikte oluþturulmuþ olacaktýr.
> Identity yapýsý içerisindeki bütün tablolar veritabaný içerisine gelmiþ olacaktýr. 
	- AspNetUsers tablosu kullanýcý bilgilerini tutar.
	- AspNetRoles tablosu sistemdeki rolleri tutar.
	- AspNetUserRoles tablosu kullanýcalara özelleþtirilmiþ þekilde atanan rolleri tutar.
> Programý bu aþamada çalýþtýrýrsak kullanýcýnýn mail adresi ve þifresi ile kayýt olup giriþ yapmasýný saðlayan **Register** ve **Login** ekranlarýna ulaþabilir ve çalýþtýrabiliriz. 

Bu adýmdan sonra projemizin identity özelliklerini geliþtirmek ve özelleþtirmek istersek kullanýcýnýn adresini, telefon numarasýný, cinsiyetini, doðum tarihini, rolünü eklemek üzere adýmlarý takip edebiliriz.
> IdentityUser sýnýfý içerisinde özellikleri tutuyor. IdentityRole sýnýfýnda ise rollendirme iþlemleri mevcut. Biz özelliklerine eklemeler yapmak istiyoruz. Bu nedenle yeni kullanýcý özelliklerini adýný CustomUser verdiðimiz yeni bir sýnýf içerisine tanýmlýyoruz.
 

     public class CustomUser : IdentityUser
        {
            [Display(Name = "Adý & Soyadý")]
            public string Fullname { get; set; }
            
            [Display(Name = "Adres")]
            public string Address { get; set; }
    
            [Display(Name = "Cinsiyet")]
    
            public string  Gender { get; set; }
    
            [Display(Name = "Doðum Tarihi")]
            [DataType(DataType.DateTime)]
    
            public DateTime DateOfBirth { get; set; }
        }

> IdentityDbContext default olarak IdentityUser sýnýfýný kullanýrken biz özelleþtirme yaparken CustomUser ile devam edeceðiz.

     public class ApplicationDbContext : IdentityDbContext<CustomUser>
        {

> Startup.cs dosyasýnda da AddDefaultIdentity sýnýfý içerisindeki IdentityUser sýnýfýný CustomUser olarak deðiþtireceðiz.

 

    services.AddDefaultIdentity<CustomUser>(options => options.SignIn.RequireConfirmedAccount = false)

> Bunlarla birlikte Shared dosyasý içindeki _LoginPartial.cshtml dosyasýnda IdentityUser sýnýfýný CustomUser ile deðiþtiriyoruz.

    @using Microsoft.AspNetCore.Identity
    @inject SignInManager<CustomUser> SignInManager
    @inject UserManager<CustomUser> UserManager


Deðiþiklilerin veritabanýna yansýmasý için Package Manager Console üzerinde **add-migration MigrationName** ve  **update-database** iþlemleri uygulanýr.

> Identity içerisine bütün sayfalar API üzerinden gelir. Bu nedenle default bir sayfayý Pages içerisinde göremezsiniz. Bu nedenle yeni bir sayfa eklemek için herhangi bir klasörden sað týklayarak **Add -> New Scaffolded Item** diyoruz. Sol taraftan identity seçeneði ile istediðimiz, bizim için "_Account/Login”, “Account/Register” ve “Account/Manage/Index_", seçenekleri ekliyoruz.
> Default register ekranýnda yalnýzca email ve þifre ile kayýt olurken biz InputModel içerisine, kullanýcý sýnýfýna eklediðimiz adý & soyadý, adres, cinsiyet ve doðum tarihi özelliklerini Register.cshtml.cs içerisine ekliyoruz.

    public class InputModel
            {
                [Required]
                [EmailAddress]
                [Display(Name = "Email")]
                public string Email { get; set; }
    
                [Required]
                [Display(Name = "Kullanýcý Adý")]
                public string UserName { get; set; }
    
                [Required]
                [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
                [DataType(DataType.Password)]
                [Display(Name = "Password")]
                public string Password { get; set; }
    
                [DataType(DataType.Password)]
                [Display(Name = "Confirm password")]
                [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
                public string ConfirmPassword { get; set; }
    
                [Display(Name = "Adres")]
                public string Address { get; set; }
    
                [Display(Name = "Cinsiyet")]
    
                public string Gender { get; set; }
    
                [Display(Name = "Doðum Tarihi")]
                [DataType(DataType.DateTime)]
    
                public DateTime DateOfBirth { get; set; }
                public string Fullname { get; internal set; }
            }

> Ayný zamanda Register.cshtml.cs içerisindeki **OnPostAsync** fonksiyonunda aþaðýdaki gibi düzenlemeler yapalým.
            

    var user = new CustomUser { UserName = Input.UserName, Email = Input.Email, Fullname = Input.Fullname, Address = Input.Address, Gender = Input.Gender, DateOfBirth = Input.DateOfBirth};
    var result = await _userManager.CreateAsync(user, Input.Password);

> _userManager.CreateAsync(user, Input.Password) kodu o kullanýcýyý ve þifresiyle sisteme kaydetmemize yarar. 
> Register html tarafýnda ise düzenlemeler þu þekilde olmalýdýr.
<form asp-route-returnUrl="@Model.ReturnUrl" method="post">

            <h4>Kayýt ol.</h4>
            <hr />
            <div asp-validation-summary="All" class="text-danger"></div>
            
            <div class="form-group">
                <label asp-for="Input.UserName"></label>
                <input asp-for="Input.UserName" class="form-control" />
                <span asp-validation-for="Input.UserName" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="Input.Fullname"></label>
                <input asp-for="Input.Fullname" class="form-control" />
                <span asp-validation-for="Input.Fullname" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="Input.Email"></label>
                <input asp-for="Input.Email" class="form-control" />
                <span asp-validation-for="Input.Email" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="Input.Address"></label>
                <input asp-for="Input.Address" class="form-control" />
                <span asp-validation-for="Input.Address" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="Input.Gender"></label>
                <select asp-for="Input.Gender">
                    <option disabled>Cinsiyet Seçiniz</option>
                    <option  value="Belirtmek istemiyorum">Belirtmek istemiyorum</option>
                    <option  value="Kadýn">Kadýn</option>
                    <option  value="Erkek">Erkek</option>
                </select>
                <span asp-validation-for="Input.Gender" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="Input.DateOfBirth"></label>
                <input asp-for="Input.DateOfBirth" class="form-control" />
                <span asp-validation-for="Input.DateOfBirth" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="Input.Password"></label>
                <input asp-for="Input.Password" class="form-control" />
                <span asp-validation-for="Input.Password" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="Input.ConfirmPassword"></label>
                <input asp-for="Input.ConfirmPassword" class="form-control" />
                <span asp-validation-for="Input.ConfirmPassword" class="text-danger"></span>
            </div>
            <button type="submit" class="btn btn-primary">Kaydol</button>
        
        </form>
            
         
> Login.cshtml.cs ve Login.cshtml sayfalarýnda da benzeri benzeri yaparak arka plandaki düzenlemeleri tamamlayabiliriz. 
> Son olarak eðer istenirse _LoginPartial kýsmýnda kullanýcýnýn sisteme giriþ yaptýðý sayfadakileri düzenleyebiliriz. 
ile

Identity ile kullanýcýdan istenen bilgiler ve giriþ yapma aþamalarý özelleþtirilebilir ve çeþitlendirmeler ile geliþtirilebilirler.


#### Kaynaklar
- [Kaynak 1](https://medium.com/kodluyoruz/asp-net-core-identity-ile-rol-bazl%C4%B1-%C3%BCyelik-sistemi-olu%C5%9Fturmak-kullan%C4%B1c%C4%B1y%C4%B1-%C3%B6zelle%C5%9Ftirmek-6c51cc31fea6)
- [Kaynak 2](https://medium.com/@gktnkrdg/https-medium-com-gktnkrdg-net-core-identity-ile-uyelik-sistemi-olusturmak-24bb9c80bf88)
- [Kaynak 3](https://docs.microsoft.com/tr-tr/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-5.0)


 
