#region Copyright
// 
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2014
// by DotNetNuke Corporation
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

#endregion
#region Usings
using System;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Users;
using DotNetNuke.Framework;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.FileSystem;
using System.Collections.Generic;


#endregion
namespace DotNetNuke.Security.Permissions
{
    public partial class FolderPermissionController : ServiceLocator<IFolderPermissionController, FolderPermissionController>, IFolderPermissionController
    {
        private static readonly PermissionProvider s_provider = PermissionProvider.Instance();

        protected override Func<IFolderPermissionController> GetFactory()
        {
            return () => new FolderPermissionController();
        }

        #region Public Methods

        /// <summary>
        /// Returns a flag indicating whether the current user can add a folder or file
        /// </summary>
        /// <param name="folder">The page</param>
        /// <returns>A flag indicating whether the user has permission</returns>
        bool IFolderPermissionController.CanAddFolder(IFolderInfo folder)
        {
            return s_provider.CanAddFolder((FolderInfo)folder);
        }

        /// <summary>
        /// Returns a flag indicating whether the current user can addmister a folder
        /// </summary>
        /// <param name="folder">The page</param>
        /// <returns>A flag indicating whether the user has permission</returns>
        bool IFolderPermissionController.CanAdminFolder(IFolderInfo folder)
        {
            return s_provider.CanAdminFolder((FolderInfo)folder);
        }

        /// <summary>
        /// Returns a flag indicating whether the current user can view a folder or file
        /// </summary>
        /// <param name="folder">The page</param>
        /// <returns>A flag indicating whether the user has permission</returns>
        bool IFolderPermissionController.CanViewFolder(IFolderInfo folder)
        {
            return s_provider.CanViewFolder((FolderInfo)folder);
        }

        #endregion

        private static void ClearPermissionCache(int PortalID)
        {
            DataCache.ClearFolderPermissionsCache(PortalID);
            DataCache.ClearCache(string.Format("Folders|{0}|", PortalID));
            DataCache.ClearFolderCache(PortalID);
        }

        /// <summary>
        /// Returns a list with all roles with implicit permissions on Folders
        /// </summary>
        /// <param name="portalId">The Portal Id where the Roles are</param>
        /// <returns>A List with the implicit roles</returns>
        public static IEnumerable<RoleInfo> ImplicitRoles(int portalId)
        {
            return s_provider.ImplicitRolesForPages(portalId);
        }

        /// <summary>
        /// Returns a flag indicating whether the current user can add a folder or file
        /// </summary>
        /// <param name="folder">The page</param>
        /// <returns>A flag indicating whether the user has permission</returns>
        public static bool CanAddFolder(FolderInfo folder)
        {
            return s_provider.CanAddFolder(folder);
        }

        /// <summary>
        /// Returns a flag indicating whether the current user can addmister a folder
        /// </summary>
        /// <param name="folder">The page</param>
        /// <returns>A flag indicating whether the user has permission</returns>
        public static bool CanAdminFolder(FolderInfo folder)
        {
            return s_provider.CanAdminFolder(folder);
        }

        /// <summary>
        /// Returns a flag indicating whether the current user can browse the folder
        /// </summary>
        /// <param name="folder">The page</param>
        /// <returns>A flag indicating whether the user has permission</returns>
        public static bool CanBrowseFolder(FolderInfo folder)
        {
            return s_provider.CanBrowseFolder(folder);
        }

        /// <summary>
        /// Returns a flag indicating whether the current user can copy a folder or file
        /// </summary>
        /// <param name="folder">The page</param>
        /// <returns>A flag indicating whether the user has permission</returns>
        public static bool CanCopyFolder(FolderInfo folder)
        {
            return s_provider.CanCopyFolder(folder);
        }

        /// <summary>
        /// Returns a flag indicating whether the current user can delete a folder or file
        /// </summary>
        /// <param name="folder">The page</param>
        /// <returns>A flag indicating whether the user has permission</returns>
        public static bool CanDeleteFolder(FolderInfo folder)
        {
            return s_provider.CanDeleteFolder(folder);
        }

        /// <summary>
        /// Returns a flag indicating whether the current user can manage a folder's settings
        /// </summary>
        /// <param name="folder">The page</param>
        /// <returns>A flag indicating whether the user has permission</returns>
        public static bool CanManageFolder(FolderInfo folder)
        {
            return s_provider.CanManageFolder(folder);
        }

        /// <summary>
        /// Returns a flag indicating whether the current user can view a folder or file
        /// </summary>
        /// <param name="folder">The page</param>
        /// <returns>A flag indicating whether the user has permission</returns>
        public static bool CanViewFolder(FolderInfo folder)
        {
            return s_provider.CanViewFolder(folder);
        }

        public static void DeleteFolderPermissionsByUser(UserInfo objUser)
        {
            s_provider.DeleteFolderPermissionsByUser(objUser);
            ClearPermissionCache(objUser.PortalID);
        }

        public static FolderPermissionCollection GetFolderPermissionsCollectionByFolder(int PortalID, string Folder)
        {
            return s_provider.GetFolderPermissionsCollectionByFolder(PortalID, Folder);
        }

        public static bool HasFolderPermission(int portalId, string folderPath, string permissionKey)
        {
            return HasFolderPermission(GetFolderPermissionsCollectionByFolder(portalId, folderPath), permissionKey);
        }

        public static bool HasFolderPermission(FolderPermissionCollection objFolderPermissions, string PermissionKey)
        {
            bool hasPermission = s_provider.HasFolderPermission(objFolderPermissions, "WRITE");
            if (!hasPermission)
            {
                if (PermissionKey.Contains(","))
                {
                    foreach (string permission in PermissionKey.Split(','))
                    {
                        if (s_provider.HasFolderPermission(objFolderPermissions, permission))
                        {
                            hasPermission = true;
                            break;
                        }
                    }
                }
                else
                {
                    hasPermission = s_provider.HasFolderPermission(objFolderPermissions, PermissionKey);
                }
            }
            return hasPermission;
        }

        /// <summary>
        /// Copies the permissions to subfolders.
        /// </summary>
        /// <param name="folder">The parent folder.</param>
        /// <param name="newPermissions">The new permissions.</param>
        public static void CopyPermissionsToSubfolders(IFolderInfo folder, FolderPermissionCollection newPermissions)
        {
            bool clearCache = CopyPermissionsToSubfoldersRecursive(folder, newPermissions);
            if (clearCache)
            {
                DataCache.ClearFolderCache(folder.PortalID);
            }
        }

        private static bool CopyPermissionsToSubfoldersRecursive(IFolderInfo folder, FolderPermissionCollection newPermissions)
        {
            bool clearCache = Null.NullBoolean;
            IEnumerable<IFolderInfo> childFolders = FolderManager.Instance.GetFolders(folder);
            foreach (var f in childFolders)
            {
                if (CanAdminFolder((FolderInfo)f))
                {
                    f.FolderPermissions.Clear();
                    f.FolderPermissions.AddRange(newPermissions);
                    SaveFolderPermissions(f);
                    clearCache = true;
                }
                clearCache = CopyPermissionsToSubfoldersRecursive(f, newPermissions) || clearCache;
            }
            return clearCache;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// SaveFolderPermissions updates a Folder's permissions
        /// </summary>
        /// <param name="folder">The Folder to update</param>
        /// <history>
        /// 	[cnurse]	04/15/2009   Created
        /// </history>
        /// -----------------------------------------------------------------------------
        public static void SaveFolderPermissions(FolderInfo folder)
        {
            SaveFolderPermissions((IFolderInfo)folder);
        }

        /// <summary>
        /// SaveFolderPermissions updates a Folder's permissions
        /// </summary>
        /// <param name="folder">The Folder to update</param>
        public static void SaveFolderPermissions(IFolderInfo folder)
        {
            s_provider.SaveFolderPermissions(folder);
            ClearPermissionCache(folder.PortalID);
        }
    }
}
