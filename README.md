# IdentityExample



# ASP.NET Core Identity ile �yelik Sistemi 

> Identity, sistem kullan�c�lar�na �yelik olu�turma, kimliklerini do�rulama ve yetkilendirme sa�layan bir sistemdir. Kullan�c�lar identity yap�s� sayesinde bilgileri ile bir hesap olu�turabilir ve olu�turulan hesaplar genellikle  SQL Server olmak �zere  PostgreSQL, MySQL, Azure Table Storage gibi veri tabanlar� �zerinde de depolanabilir.
> K�saca identity i�in kullan�c� aray�z� (UI) giri� i�levini destekleyen bir API denebilir.
> .NET Core Identity Scaffolding bu �yelik sistemi i�in hem aray�z hem de kay�t, giri� gibi k�s�mlar i�in gerekli kod bloklar�n� [Razor Pages](https://docs.microsoft.com/tr-tr/aspnet/core/razor-pages/?view=aspnetcore-3.0) ile olu�turur.

### Bu �rnek projede de **Identity** yap�s� kullan�larak kimlik do�rulamas�na sahip bir web uygulamas� geli�tirilmi�tir.  
##### Dok�man�n devam�nda �rnek ad�m ad�m anlat�lacakt�r. 
> �ncelikle Visual Studio ortam�nda yeni bir ASP.NET Core Web Application (C#) projesi olu�tural�m. Kar��m�za gelen ekranda "No Authentication" se�ene�ini **"Individual User Accounts"** �eklinde de�i�tiriyor ve b�ylece projemizi olu�turuyoruz. 
![Create Application](https://m2c6a4u3.stackpathcdn.com/wp-content/uploads/2020/03/creating-aspnet-core-mvc-with-identity-selected.jpg )

> Identity sayfalar�n�n tutuldu�u **Areas** ve  haz�r DBContext ve Migration �n yerle�ti�i **Data** dosyalar� bu yap�yla birlikte MVC projesi i�erisine gelmi�tir.  
![](https://i0.wp.com/tutexchange.com/wp-content/uploads/2020/01/image-135.png?resize=311%2C339&ssl=1)

> Bunlarla birlikte olu�an app.setting.json dosyas�nda otomatik olarak bir Connection String olu�turulmu�tur. 

    {    
    {  "ConnectionStrings": {  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=aspnet-IEExample-4FC24192-AEFC-4273-80AF-A4C1AD922062;Trusted_Connection=True;MultipleActiveResultSets=true"
      }

> Startup.cs i�erisinde Sql Server kullan�larak veritaban� ayarlar� ConfigureServices metotu i�erisinde tan�mlanm��t�r.

   
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

> Buradaki AddDefaultIdentity IdentityUser s�n�f� ile identity ayarlar�n� servise kaydetmeye yarar.
> Zaten otomatik olarak olu�turulmu� bu yap�lar�n yan�nda yap�lmas� gereken i�lem migration ve ayarlar� veritaban�na yans�tmak olacakt�r. Bunun i�in Package Manager Console �zerinde **update-database** i�lemi uygulan�r. B�ylece veritaban� ayarlar�yla birlikte olu�turulmu� olacakt�r.
> Identity yap�s� i�erisindeki b�t�n tablolar veritaban� i�erisine gelmi� olacakt�r. 
	- AspNetUsers tablosu kullan�c� bilgilerini tutar.
	- AspNetRoles tablosu sistemdeki rolleri tutar.
	- AspNetUserRoles tablosu kullan�calara �zelle�tirilmi� �ekilde atanan rolleri tutar.
> Program� bu a�amada �al��t�r�rsak kullan�c�n�n mail adresi ve �ifresi ile kay�t olup giri� yapmas�n� sa�layan **Register** ve **Login** ekranlar�na ula�abilir ve �al��t�rabiliriz. 

Bu ad�mdan sonra projemizin identity �zelliklerini geli�tirmek ve �zelle�tirmek istersek kullan�c�n�n adresini, telefon numaras�n�, cinsiyetini, do�um tarihini, rol�n� eklemek �zere ad�mlar� takip edebiliriz.
> IdentityUser s�n�f� i�erisinde �zellikleri tutuyor. IdentityRole s�n�f�nda ise rollendirme i�lemleri mevcut. Biz �zelliklerine eklemeler yapmak istiyoruz. Bu nedenle yeni kullan�c� �zelliklerini ad�n� CustomUser verdi�imiz yeni bir s�n�f i�erisine tan�ml�yoruz.
 

     public class CustomUser : IdentityUser
        {
            [Display(Name = "Ad� & Soyad�")]
            public string Fullname { get; set; }
            
            [Display(Name = "Adres")]
            public string Address { get; set; }
    
            [Display(Name = "Cinsiyet")]
    
            public string  Gender { get; set; }
    
            [Display(Name = "Do�um Tarihi")]
            [DataType(DataType.DateTime)]
    
            public DateTime DateOfBirth { get; set; }
        }

> IdentityDbContext default olarak IdentityUser s�n�f�n� kullan�rken biz �zelle�tirme yaparken CustomUser ile devam edece�iz.

     public class ApplicationDbContext : IdentityDbContext<CustomUser>
        {

> Startup.cs dosyas�nda da AddDefaultIdentity s�n�f� i�erisindeki IdentityUser s�n�f�n� CustomUser olarak de�i�tirece�iz.

 

    services.AddDefaultIdentity<CustomUser>(options => options.SignIn.RequireConfirmedAccount = false)

> Bunlarla birlikte Shared dosyas� i�indeki _LoginPartial.cshtml dosyas�nda IdentityUser s�n�f�n� CustomUser ile de�i�tiriyoruz.

    @using Microsoft.AspNetCore.Identity
    @inject SignInManager<CustomUser> SignInManager
    @inject UserManager<CustomUser> UserManager


De�i�iklilerin veritaban�na yans�mas� i�in Package Manager Console �zerinde **add-migration MigrationName** ve  **update-database** i�lemleri uygulan�r.

> Identity i�erisine b�t�n sayfalar API �zerinden gelir. Bu nedenle default bir sayfay� Pages i�erisinde g�remezsiniz. Bu nedenle yeni bir sayfa eklemek i�in herhangi bir klas�rden sa� t�klayarak **Add -> New Scaffolded Item** diyoruz. Sol taraftan identity se�ene�i ile istedi�imiz, bizim i�in "_Account/Login�, �Account/Register� ve �Account/Manage/Index_", se�enekleri ekliyoruz.
> Default register ekran�nda yaln�zca email ve �ifre ile kay�t olurken biz InputModel i�erisine, kullan�c� s�n�f�na ekledi�imiz ad� & soyad�, adres, cinsiyet ve do�um tarihi �zelliklerini Register.cshtml.cs i�erisine ekliyoruz.

    public class InputModel
            {
                [Required]
                [EmailAddress]
                [Display(Name = "Email")]
                public string Email { get; set; }
    
                [Required]
                [Display(Name = "Kullan�c� Ad�")]
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
    
                [Display(Name = "Do�um Tarihi")]
                [DataType(DataType.DateTime)]
    
                public DateTime DateOfBirth { get; set; }
                public string Fullname { get; internal set; }
            }

> Ayn� zamanda Register.cshtml.cs i�erisindeki **OnPostAsync** fonksiyonunda a�a��daki gibi d�zenlemeler yapal�m.
            

    var user = new CustomUser { UserName = Input.UserName, Email = Input.Email, Fullname = Input.Fullname, Address = Input.Address, Gender = Input.Gender, DateOfBirth = Input.DateOfBirth};
    var result = await _userManager.CreateAsync(user, Input.Password);

> _userManager.CreateAsync(user, Input.Password) kodu o kullan�c�y� ve �ifresiyle sisteme kaydetmemize yarar. 
> Register html taraf�nda ise d�zenlemeler �u �ekilde olmal�d�r.
<form asp-route-returnUrl="@Model.ReturnUrl" method="post">

            <h4>Kay�t ol.</h4>
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
                    <option disabled>Cinsiyet Se�iniz</option>
                    <option  value="Belirtmek istemiyorum">Belirtmek istemiyorum</option>
                    <option  value="Kad�n">Kad�n</option>
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
            
         
> Login.cshtml.cs ve Login.cshtml sayfalar�nda da benzeri benzeri yaparak arka plandaki d�zenlemeleri tamamlayabiliriz. 
> Son olarak e�er istenirse _LoginPartial k�sm�nda kullan�c�n�n sisteme giri� yapt��� sayfadakileri d�zenleyebiliriz. 
ile

Identity ile kullan�c�dan istenen bilgiler ve giri� yapma a�amalar� �zelle�tirilebilir ve �e�itlendirmeler ile geli�tirilebilirler.


#### Kaynaklar
- [Kaynak 1](https://medium.com/kodluyoruz/asp-net-core-identity-ile-rol-bazl%C4%B1-%C3%BCyelik-sistemi-olu%C5%9Fturmak-kullan%C4%B1c%C4%B1y%C4%B1-%C3%B6zelle%C5%9Ftirmek-6c51cc31fea6)
- [Kaynak 2](https://medium.com/@gktnkrdg/https-medium-com-gktnkrdg-net-core-identity-ile-uyelik-sistemi-olusturmak-24bb9c80bf88)
- [Kaynak 3](https://docs.microsoft.com/tr-tr/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-5.0)


 
