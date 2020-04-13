# Twitter-API
API Twitter : TEST
#Desenvolvedor: ATILIO CAMARGO MOREIRA

Este projeto básico foi feito usando:
# NET CORE
# Chamadas assíncronas (Async)
# Session
# Jquery (Busca Parametrizada)
# IOptions Pattern
# Exportação para CSV

EndPoints configurados:
https://api.twitter.com/oauth2/token  (Obter o Token)
https://api.twitter.com/1.1/statuses/user_timeline.json (Buscar toda timeline de acordo com a qtde parametrizada)
https://api.twitter.com/1.1/search/tweets.json (Buscar os twitts de acordo com parametro informado pelo usuario)

Implementações importantes:

          ---ConfigureServices
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var identitySettingsSection =
                Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(identitySettingsSection);
            
            ----- Configure
            app.UseSession();
