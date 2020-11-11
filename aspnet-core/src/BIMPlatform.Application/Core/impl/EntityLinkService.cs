using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace BIMPlatform.Core.impl
{
    public class EntityLinkService :  IEntityLinkService
    {
        public EntityLinkService()
        {
        }

        //public IList<object> GetMasterLinkEntities(EntityDataInfo entityInfo, string linkType = "")
        //{
        //   // // ServerLogger.Info(string.Format("Get master link entities by sub entity from {0} by {1}-{2}", entityInfo.EntityClassName, entityInfo.EntityKey, entityInfo.EntityValue));
        //    Stopwatch watch = new Stopwatch();
        //    watch.Start();

        //    try
        //    {
        //        IList<EntityLink> masterEntityLinks = GetMasterLinksInternal(entityInfo, linkType);
        //        IEnumerable<IGrouping<string, EntityLink>> masterEntityGroups = masterEntityLinks.GroupBy(e => e.FromEntityClassName);
        //        return GetLinkEntities(masterEntityGroups, true);
        //    }
        //    finally
        //    {
        //        watch.Stop();
        //        // ServerLogger.Perfomance(watch, "Get master link entities");
        //       // // ServerLogger.Info("End to get master link entities");
        //    }
        //}

        //public IList<object> GetSubLinkEntities(EntityDataInfo entityInfo, string linkType = "")
        //{
        //   // // ServerLogger.Info(string.Format("Get sub link entities by master entity from {0} by {1}-{2}", entityInfo.EntityClassName, entityInfo.EntityKey, entityInfo.EntityValue));
        //    Stopwatch watch = new Stopwatch();
        //    watch.Start();

        //    try
        //    {
        //        IList<EntityLink> subEntityLinks = GetSubLinksInternal(entityInfo, linkType);
        //        IEnumerable<IGrouping<string, EntityLink>> subEntityGroups = subEntityLinks.GroupBy(e => e.ToEntityClassName);

        //        return GetLinkEntities(subEntityGroups, false);
        //    }
        //    finally
        //    {
        //        watch.Stop();
        //        // ServerLogger.Perfomance(watch, "Get sub link entities");
        //       // // ServerLogger.Info("End to get sub link entities");
        //    }
        //}

        //public IList<EntityLink> GetMasterLinksInternal(EntityDataInfo entityInfo, string linkType = "")
        //{
        //    IList<EntityLink> masterEntityLinks = null;
        //    if (string.IsNullOrEmpty(linkType))
        //    {
        //        masterEntityLinks = EntityLinkRepository.FindList(link => link.ToEntityClassName == entityInfo.EntityClassName && link.ToEntityKey == entityInfo.EntityKey && link.ToEntityValue == entityInfo.EntityValue);
        //    }
        //    else
        //    {
        //        masterEntityLinks = EntityLinkRepository.FindList(link => link.ToEntityClassName == entityInfo.EntityClassName && link.ToEntityKey == entityInfo.EntityKey && link.ToEntityValue == entityInfo.EntityValue && link.Type == linkType);
        //    }
        //    return masterEntityLinks;
        //}

        //public IList<EntityLink> GetSubLinksInternal(EntityDataInfo entityInfo, string linkType = "")
        //{
        //    IList<EntityLink> subEntityLinks = null;
        //    if (string.IsNullOrEmpty(linkType))
        //    {
        //        subEntityLinks = EntityLinkRepository.FindList(link => link.FromEntityClassName == entityInfo.EntityClassName && link.FromEntityKey == entityInfo.EntityKey && link.FromEntityValue == entityInfo.EntityValue);
        //    }
        //    else
        //    {
        //        subEntityLinks = EntityLinkRepository.FindList(link => link.FromEntityClassName == entityInfo.EntityClassName && link.FromEntityKey == entityInfo.EntityKey && link.FromEntityValue == entityInfo.EntityValue && link.Type == linkType);
        //    }
        //    return subEntityLinks;
        //}
        //public void LinkEntity(AssociationEntityDataInfo fromEntityInfo, IList<AssociationEntityDataInfo> toEntityInfos, string type)
        //{
        //    Stopwatch watch = new Stopwatch();
        //    watch.Start();

        //    try
        //    {
        //        if (fromEntityInfo == null)
        //            throw new ArgumentNullException("masterEntityInfo");

        //        if (string.IsNullOrEmpty(type))
        //            throw new ArgumentNullException("type");

        //        if (string.IsNullOrEmpty(fromEntityInfo.EntityClassName) || string.IsNullOrEmpty(fromEntityInfo.EntityKey) ||
        //            string.IsNullOrEmpty(fromEntityInfo.EntityValue))
        //            throw new ArgumentException("Entity misses values");

        //       // // ServerLogger.Info(string.Format("Start to link entities to entity {0}-{1}-{2} with type {3}", fromEntityInfo.EntityClassName, fromEntityInfo.EntityKey, fromEntityInfo.EntityValue, type));

        //        bool addRel = false;
        //        foreach (AssociationEntityDataInfo subEntity in toEntityInfos)
        //        {
        //            // Link document
        //            EntityLink linkRel = EntityLinkRepository.FindByKeyValues(fromEntityInfo.EntityClassName, fromEntityInfo.EntityKey, fromEntityInfo.EntityValue,
        //                                                                       subEntity.EntityClassName, subEntity.EntityKey, subEntity.EntityValue, type);
        //            if (linkRel == null)
        //            {
        //                linkRel = new EntityLink()
        //                {
        //                    FromEntityClassName = fromEntityInfo.EntityClassName,
        //                    FromEntityKey = fromEntityInfo.EntityKey,
        //                    FromEntityValue = fromEntityInfo.EntityValue,
        //                    ToEntityClassName = subEntity.EntityClassName,
        //                    ToEntityKey = subEntity.EntityKey,
        //                    ToEntityValue = subEntity.EntityValue,
        //                    Type = type,
        //                    Description= subEntity.Description,
        //                    CreationUserID = fromEntityInfo.UserID,
        //                    CreationDate = DateTime.Now,
        //                };

        //                EntityLinkRepository.Add(linkRel);
        //                addRel = true;
        //            }
        //        }

        //        if (addRel)
        //            this.Context.Save();
        //    }
        //    finally
        //    {
        //        watch.Stop();
        //        // ServerLogger.Perfomance(watch, "Link entities");
        //       // // ServerLogger.Info("End to link entity");
        //    }
        //}

        //public void RemoveEntityByFromEntity(EntityDataInfo fromEntityInfo, IList<EntityDataInfo> toEntityInfosInSameType, string linkType = "")
        //{
        //    IList<EntityLink> linkToRels = null;
        //    if (string.IsNullOrEmpty(linkType))
        //    {
        //        if (toEntityInfosInSameType != null && toEntityInfosInSameType.Count > 0)
        //        {
        //            EntityDataInfo firstItem = toEntityInfosInSameType.First();
        //            IEnumerable<string> values = toEntityInfosInSameType.Select(e => e.EntityValue);
        //            linkToRels = EntityLinkRepository.FindList(e => e.FromEntityClassName == fromEntityInfo.EntityClassName &&
        //                            e.FromEntityKey == fromEntityInfo.EntityKey && e.FromEntityValue == fromEntityInfo.EntityValue &&
        //                            e.ToEntityClassName == firstItem.EntityClassName && e.ToEntityKey == firstItem.EntityKey &&
        //                            values.Contains(e.ToEntityValue));
        //        }
        //        else
        //        {
        //            linkToRels = EntityLinkRepository.FindList(e => e.FromEntityClassName == fromEntityInfo.EntityClassName &&
        //                            e.FromEntityKey == fromEntityInfo.EntityKey && e.FromEntityValue == fromEntityInfo.EntityValue);
        //        }
        //    }
        //    else
        //    {
        //        if (toEntityInfosInSameType != null && toEntityInfosInSameType.Count > 0)
        //        {
        //            EntityDataInfo firstItem = toEntityInfosInSameType.First();
        //            IEnumerable<string> values = toEntityInfosInSameType.Select(e => e.EntityValue);
        //            linkToRels = EntityLinkRepository.FindList(e => e.FromEntityClassName == fromEntityInfo.EntityClassName &&
        //                        e.FromEntityKey == fromEntityInfo.EntityKey && e.FromEntityValue == fromEntityInfo.EntityValue &&
        //                        e.ToEntityClassName == firstItem.EntityClassName && e.ToEntityKey == firstItem.EntityKey &&
        //                        values.Contains(e.ToEntityValue) && e.Type == linkType);
        //        }
        //        else
        //        {
        //            linkToRels = EntityLinkRepository.FindList(e => e.FromEntityClassName == fromEntityInfo.EntityClassName &&
        //                        e.FromEntityKey == fromEntityInfo.EntityKey && e.FromEntityValue == fromEntityInfo.EntityValue && e.Type == linkType);
        //        }
        //    }
        //    if (linkToRels != null)
        //    {
        //        EntityLinkRepository.Delete(linkToRels);
        //    }
        //    this.Context.Save();
        //}

        //public IList<object> GetLinkEntities(IEnumerable<IGrouping<string, EntityLink>> entitiesByClass, bool getMaster)
        //{
        //    List<object> associatedEntities = new List<object>();

        //    foreach (IGrouping<string, EntityLink> group in entitiesByClass)
        //    {
        //        Dictionary<string, IList<EntityLink>> entityKeyToEntities = new Dictionary<string, IList<EntityLink>>();
        //        foreach (EntityLink entity in group)
        //        {
        //            //string key = getMaster ? entity.FromEntityKey + "_" + entity.FromEntityValue : entity.ToEntityKey + "_" + entity.ToEntityValue;
        //            string key = getMaster ? entity.FromEntityKey : entity.ToEntityKey;
        //            if (!entityKeyToEntities.ContainsKey(key))
        //            {
        //                entityKeyToEntities[key] = new List<EntityLink>();
        //            }

        //            entityKeyToEntities[key].Add(entity);
        //        }

        //        foreach (KeyValuePair<string, IList<EntityLink>> kvp in entityKeyToEntities)
        //        {
        //            EntityLink first = kvp.Value.First();
        //            string table = getMaster ? first.FromEntityClassName : first.ToEntityClassName;
        //            string keyProperty = getMaster ? first.FromEntityKey : first.ToEntityKey;
        //            IEnumerable<string> values = getMaster ? kvp.Value.Select(e => e.FromEntityValue) : kvp.Value.Select(e => e.ToEntityValue);
        //            IEnumerable<object> results = this.GeneralQueryEntities(table, keyProperty, values);
        //            associatedEntities.AddRange(results);
        //        }
        //    }

        //    return associatedEntities;
        //}

        //public void OnEntityRemoved(RemovedEntityDataInfo entityInfo)
        //{
        //    IList<BaseModule> activedModules = ApplicationService.Instance.GetActivedModules();

        //    IList<EntityLink> linkToRels = EntityLinkRepository.FindList(e => e.FromEntityClassName == entityInfo.EntityClassName && e.FromEntityKey == entityInfo.EntityKey && e.FromEntityValue == entityInfo.EntityValue);
        //    DeleteLinks(linkToRels, entityInfo.OperationUserID);

        //    IList<EntityLink> linkFromRels = EntityLinkRepository.FindList(e => e.ToEntityClassName == entityInfo.EntityClassName && e.ToEntityKey == entityInfo.EntityKey && e.ToEntityValue == entityInfo.EntityValue);
        //    DeleteLinks(linkFromRels, entityInfo.OperationUserID);
            
        //    this.Context.Save();
        //}

        //private void DeleteLinks(IList<EntityLink> linkToRels, int userID)
        //{
        //    IList<BaseModule> activedModules = ApplicationService.Instance.GetActivedModules();

        //    if (linkToRels != null)
        //    {
        //        foreach (EntityLink link in linkToRels)
        //        {
        //            bool handled = false;

        //            if (activedModules != null)
        //            {
        //                foreach (BaseModule module in activedModules)
        //                {
        //                    bool specicalHandled = module.HandleEntityLinkRemovalOfSpecicalType(link, userID);
        //                    if (specicalHandled)
        //                    {
        //                        handled = true;
        //                    }
        //                }
        //            }

        //            // Delete the link totally if not handled by any module
        //            if (!handled)
        //                EntityLinkRepository.Delete(link);
        //        }
        //    }
        //}

        //public void UpdateToEntityValue(EntityDataInfo fromEntityInfo, EntityDataInfo toEntityInfo,string toEntityValue)
        //{
        //    EntityLink link = EntityLinkRepository.FirstOrDefault(e => e.FromEntityClassName == fromEntityInfo.EntityClassName &&
        //                       e.FromEntityKey == fromEntityInfo.EntityKey && e.FromEntityValue == fromEntityInfo.EntityValue &&
        //                       e.ToEntityClassName == toEntityInfo.EntityClassName && e.ToEntityKey == toEntityInfo.EntityKey &&
        //                       e.ToEntityValue== toEntityInfo.EntityValue);
        //    if (link != null && !string.IsNullOrEmpty(toEntityValue))
        //    {
        //        link.ToEntityValue = toEntityValue;
        //        EntityLinkRepository.Update(link);
        //    }
        //    this.Context.Save();
        //}

        //public void UpdateEntityLinkInternal(EntityLink linkRelEntity)
        //{
        //    if (linkRelEntity != null)
        //    {
        //        EntityLinkRepository.Update(linkRelEntity);
        //        this.Context.Save();
        //    }
        //}
    }
}
