namespace Sitecore.Support.XA.Foundation.Variants.Abstractions.Pipelines.GetVariants
{
  using Sitecore.Data.Items;
  using Sitecore.XA.Foundation.Multisite;
  using Sitecore.XA.Foundation.Presentation;
  using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
  using Sitecore.XA.Foundation.Variants.Abstractions.Pipelines.GetVariants;
  using System.Collections.Generic;
  using System.Linq;

  public class GetSharedVariants
  {
    private readonly ISharedSitesContext SharedSiteContext;

    private readonly IPresentationContext PresentationContext;

    public GetSharedVariants(ISharedSitesContext sharedSiteContext, IPresentationContext presentationContext)
    {
      SharedSiteContext = sharedSiteContext;
      PresentationContext = presentationContext;
    }

    public void Process(GetVariantsArgs args)
    {
      if (SharedSiteContext != null && PresentationContext != null)
      {
        Item[] sharedSitesWithoutCurrent = SharedSiteContext.GetSharedSitesWithoutCurrent(args.ContextItem);
        foreach (Item item2 in sharedSitesWithoutCurrent)
        {
          Item presentationItem = PresentationContext.GetPresentationItem(item2);
          if (presentationItem != null)
          {
            Item item3 = presentationItem.FirstChildInheritingFrom(Sitecore.XA.Foundation.Variants.Abstractions.Templates.VariantsGrouping.ID)?.Children.FirstOrDefault((Item item) => item.Name.Equals(args.RenderingName));
            if (item3 != null)
            {
              args.Variants.AddRange(item3.Children);
            }

            #region Added code
            item3 = presentationItem.FirstChildInheritingFrom(Sitecore.XA.Foundation.Variants.Abstractions.Templates.VariantsGrouping.ID);
            List<Item> list = new List<Item>();
            if (item3 != null)
            {
              foreach (Item child in item3.Children)
              {
                if (((BaseItem)child)[Sitecore.XA.Foundation.Variants.Abstractions.Templates.ICompatibleRenderings.Fields.CompatibleRenderings].Contains(args.RenderingId.ToString()))
                {
                  list.AddRange(child.Children);
                }
                else
                {
                  list.AddRange(from variantRoot in child.Children
                                where ((BaseItem)variantRoot)[Sitecore.XA.Foundation.Variants.Abstractions.Templates.ICompatibleRenderings.Fields.CompatibleRenderings].Contains(args.RenderingId.ToString())
                                select variantRoot);
                }
              }
              args.Variants.AddRange(list);
            }
            #endregion

          }
        }
      }
    }
  }
}