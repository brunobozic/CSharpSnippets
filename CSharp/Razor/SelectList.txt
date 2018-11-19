@Html.DropDownList("FooBarDropDown", new List<SelectListItem>
{
    new SelectListItem{ Text="Option 1", Value = "1" },
    new SelectListItem{ Text="Option 2", Value = "2" },
    new SelectListItem{ Text="Option 3", Value = "3" },
 }) 

public ActionResult ExampleView()
{
    var list = new List<SelectListItem>
    {
        new SelectListItem{ Text="Option 1", Value = "1" },
        new SelectListItem{ Text="Option 2", Value = "2" },
        new SelectListItem{ Text="Option 3", Value = "3", Selected = true },
    }); 

    ViewData["foorBarList"] = list;
    return View();
}

@Html.DropDownList("fooBarDropDown", ViewData["list"] as List<SelectListItem>)


public static class DropDownListUtility
{   
    public static IEnumerable<SelectListItem> GetFooBarDropDown(object selectedValue)
    {
        return new List<SelectListItem>
        {
            new SelectListItem{ Text="Option 1", Value = "1", Selected = "1" == selectedValue.ToString()},
            new SelectListItem{ Text="Option 2", Value = "2", Selected = "2" == selectedValue.ToString()},
            new SelectListItem{ Text="Option 3", Value = "3", Selected = "3" == selectedValue.ToString()},
        };             
    }

public ActionResult ExampleView()
{
    var list = DropDownListUtility.GetFooBarDropDown("2"); //select second option by default;
    ViewData["foorBarList"] = list;
    return View();
}

@Html.DropDownList("fooBarDropDown", DropDownListUtility.GetFooBarDropDown("2"))


//
//
//
var service = new AssesmentCriteriaService();
locKrItems = service.GetAllAssesmentCriteria().Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString() ,
                        Text = c.Label ,
                        // Selected = listOfSelectedItems.Contains(c.Id) ,
                        // Disabled = listOfSelectedItems.Contains(c.Id) // this will make the dropdown checkboxes disabled for all those items that already exist in the db, to prevent deletion
                    }).ToList();
                    
var msl = new MultiSelectList(locKrItems , "Value" , "Text" , listOfSelectedItems , listOfSelectedItems);
