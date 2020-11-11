﻿using BIMPlatform.Users;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Threading;

namespace BIMPlatform.EntityFrameworkCore
{
    public static class BIMPlatformEfCoreEntityExtensionMappings
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                /* You can configure entity extension properties for the
                 * entities defined in the used modules.
                 *
                 * The properties defined here becomes table fields.
                 * If you want to use the ExtraProperties dictionary of the entity
                 * instead of creating a new field, then define the property in the
                 * BIMPlatformDomainObjectExtensions class.
                 *
                 * Example:
                 *
                 * ObjectExtensionManager.Instance
                 *    .MapEfCoreProperty<IdentityUser, string>(
                 *        "MyProperty",
                 *        b => b.HasMaxLength(128)
                 *    );
                 *
                 * See the documentation for more:
                 * https://docs.abp.io/en/abp/latest/Customizing-Application-Modules-Extending-Entities
                 */
                ObjectExtensionManager.Instance
                .MapEfCoreProperty<IdentityUser, bool>(
                    nameof(AppUser.IsActivated),
                    b => { b.HasMaxLength(1); }
                )
                .MapEfCoreProperty<IdentityUser, string>(
                    nameof(AppUser.UserHeadImgUrl),
                    b => { b.HasMaxLength(100); }
             );
                ObjectExtensionManager.Instance
                .MapEfCoreProperty<IdentityRole, string>("RoleType", b => b.HasMaxLength(10));
            });
        }
    }
}
