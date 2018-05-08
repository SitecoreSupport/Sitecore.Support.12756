namespace Sitecore.Support.XA.Foundation.Variants.Abstractions.Pipelines.GetVariants
{
  using Sitecore.Data.Items;
  using Sitecore.XA.Foundation.Multisite;
  using Sitecore.XA.Foundation.Presentation;
  using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
  using Sitecore.XA.Foundation.Variants.Abstractions.Pipelines.GetVariants;
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
          }
        }
      }
    }
  }
}