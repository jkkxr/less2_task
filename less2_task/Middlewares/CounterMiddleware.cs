using less2_task.Services;
using less2_task.Models;

namespace less2_task.Middlewares
{
    public class CounterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDataProvider<RouteStat> _dataProvider;
        public CounterMiddleware(RequestDelegate next, IDataProvider<RouteStat> dataProvider)
        {
            _next = next;
            _dataProvider = dataProvider;
        }
        public async Task InvokeAsync(HttpContext context)
        { 
            var routes = await _dataProvider.GetAllAsync();
            string path = context.Request.Path;
            string method = context.Request.Method;

            var currRoute = routes.FirstOrDefault(r => r.RouteName == path && r.Method == method);
            if (currRoute != null)
            {
                currRoute.Count++;
            }
            else
            {
                var newRoute = new RouteStat
                {
                    RouteName = path,
                    Method = method,
                    Count = 1
                };
                routes.Add(newRoute);
            }

            await _dataProvider.SaveChanges(routes);
            await _next(context);
        }
    }
}
