using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Blazorise;
using Contract;
using Core.Enum;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Radzen;
using WebClient.Exceptions;

namespace WebClient.Shared
{

    public abstract  class BaseBlazorPage : ComponentBase
    {
        
          [Inject] private NavigationManager _navigationManager { get; set;}
          [Inject] private NotificationService _notificationService { get; set;}

      
        protected IMapper ObjectMapper { get;}
        
        public BaseBlazorPage()
        {
            
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            ObjectMapper = config.CreateMapper();
        }
        

        public async Task<bool> InvokeAsync(Func<Task> action, ActionType type, bool showNotification = false)
        {
         
            try
            {
                await action();

                if (showNotification)
                {
                    switch (type)
                    {
                        case ActionType.Create:
                        {
                            _notificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Tạo Thành Công", Duration = 4000 });
                            break;
                        }
                        case ActionType.Update:
                        {
                            _notificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Cập Nhật Thành cộng", Duration = 4000 });
                            break;
                        }
                        case ActionType.Get:
                        {
                            _notificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Lấy về Thành cộng", Duration = 4000 });
                            break;
                        }
                        case ActionType.GetList:
                        {
                            _notificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Lấy về Thành cộng", Duration = 4000 });
                            break;
                        }
                        case ActionType.Delete:
                        {
                            _notificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Xóa Thành cộng", Duration = 4000 });
                            break;
                        }
                    
                    }
                }
                
                
                return true;
            }
            catch (Exception e)
            {
                var exceptionType = e.GetType();

                if (exceptionType == typeof(BadRequestException))
                {
                    _notificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error Summary", Detail = e.Message, Duration = 4000 });

                }

                if (exceptionType == typeof(ServerErrorException))
                {
                    _navigationManager.NavigateTo("server-error",true);
                }
                
            }

            return false;

        }
        
        
        
    }
}