using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Acme.SimpleTaskApp.Authorization;

public class SimpleTaskAppAuthorizationProvider : AuthorizationProvider
{
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
        // ← stops duplicate registration error
        if (context.GetPermissionOrNull(PermissionNames.Pages_Tasks) != null)
            return;

        context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
        context.CreatePermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));
        context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
        context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);

        // Parent permission — Pages.Tasks
        var tasks = context.CreatePermission(
            PermissionNames.Pages_Tasks,
            L("Tasks")
        );

        // Child permissions
        tasks.CreateChildPermission(
            PermissionNames.Pages_Tasks_Create,
            L("CreateTask")
        );

        tasks.CreateChildPermission(
            PermissionNames.Pages_Tasks_Edit,
            L("EditTask")
        );

        tasks.CreateChildPermission(
            PermissionNames.Pages_Tasks_Delete,
            L("DeleteTask")
        );
    }

    private static ILocalizableString L(string name)
    {
        return new LocalizableString(name, SimpleTaskAppConsts.LocalizationSourceName);
    }
}
