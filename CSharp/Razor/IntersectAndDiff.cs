// intersect them with the ACs gotten from the submit form to get the items that we should persist
                    int[] intersection = null;

                    if (integerFormattedExistingResults != null && integerFormattedExistingResults.Length > 0 && aCs != null && aCs.Count > 0)
                    {
                        intersection = integerFormattedExistingResults.Intersect(aCs).ToArray();
                    }
                    // find which ACs we dont have in the Db
                    int[] onlyInArray2 = null;
                    if (aCs != null && aCs.Count > 0)
                    {
                         onlyInArray2 = aCs.Except(integerFormattedExistingResults).ToArray();
                    }

                    var counterReal = 0; // assesment criteria counter

                    var myAuditDetail = auditResult2.AuditOperationalProgramme.Select(i => i.AuditDetail)
                        .FirstOrDefault();

                    var itemsToAdd = new List<int>();

                    if (intersection != null && (onlyInArray2 != null && onlyInArray2.Length == 0 && intersection.Length > 0))
                    {
                        itemsToAdd.AddRange(intersection);
                    }
                    else if (onlyInArray2 != null && onlyInArray2.Length > 0)
                    {
                        itemsToAdd.AddRange(onlyInArray2.ToList());
                    }