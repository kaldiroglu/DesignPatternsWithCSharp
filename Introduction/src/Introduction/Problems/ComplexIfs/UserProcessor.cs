namespace dev.kaldiroglu.Introduction.Problems.ComplexIfs;

public class UserProcessor
{
    /// <summary>
    /// A ridiculously complicated method that processes user data
    /// with deeply nested if statements - an example of what NOT to do!
    /// </summary>
    public string ProcessUser(User? user, bool isAdmin, string? userType,
                              int accessLevel, bool hasPermissions,
                              string? department, bool isActive)
    {

        if (user != null)
        {
            if (user.Age >= 18)
            {
                if (isActive)
                {
                    if (userType != null)
                    {
                        if (userType == "PREMIUM")
                        {
                            if (isAdmin)
                            {
                                if (accessLevel > 5)
                                {
                                    if (hasPermissions)
                                    {
                                        if (department != null)
                                        {
                                            if (department == "IT")
                                            {
                                                if (user.Experience > 2)
                                                {
                                                    if (user.Certifications!.Count > 3)
                                                    {
                                                        if (user.LastLoginDays < 30)
                                                        {
                                                            if (user.FailedLogins < 3)
                                                            {
                                                                return "FULL_ACCESS_GRANTED";
                                                            }
                                                            else
                                                            {
                                                                if (user.FailedLogins < 5)
                                                                {
                                                                    return "LIMITED_ACCESS_SECURITY_REVIEW";
                                                                }
                                                                else
                                                                {
                                                                    return "ACCESS_BLOCKED_TOO_MANY_FAILURES";
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (user.LastLoginDays < 90)
                                                            {
                                                                return "ACCESS_GRANTED_PASSWORD_RESET_REQUIRED";
                                                            }
                                                            else
                                                            {
                                                                return "ACCOUNT_DORMANT_REACTIVATION_NEEDED";
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (user.Certifications!.Count > 1)
                                                        {
                                                            return "PARTIAL_ACCESS_CERTIFICATION_PENDING";
                                                        }
                                                        else
                                                        {
                                                            return "TRAINING_REQUIRED";
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (user.Experience > 1)
                                                    {
                                                        return "SUPERVISED_ACCESS_ONLY";
                                                    }
                                                    else
                                                    {
                                                        return "INTERN_ACCESS_BASIC_ONLY";
                                                    }
                                                }
                                            }
                                            else if (department == "HR")
                                            {
                                                if (user.BackgroundCheck)
                                                {
                                                    if (user.ComplianceTraining)
                                                    {
                                                        return "HR_FULL_ACCESS";
                                                    }
                                                    else
                                                    {
                                                        return "HR_LIMITED_COMPLIANCE_TRAINING_NEEDED";
                                                    }
                                                }
                                                else
                                                {
                                                    return "HR_ACCESS_DENIED_BACKGROUND_CHECK";
                                                }
                                            }
                                            else if (department == "FINANCE")
                                            {
                                                if (user.FinancialClearance)
                                                {
                                                    if (user.AuditScore > 85)
                                                    {
                                                        return "FINANCE_FULL_ACCESS";
                                                    }
                                                    else
                                                    {
                                                        if (user.AuditScore > 70)
                                                        {
                                                            return "FINANCE_RESTRICTED_ACCESS";
                                                        }
                                                        else
                                                        {
                                                            return "FINANCE_AUDIT_REQUIRED";
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    return "FINANCE_CLEARANCE_PENDING";
                                                }
                                            }
                                            else
                                            {
                                                if (user.GeneralTraining)
                                                {
                                                    return "GENERAL_DEPARTMENT_ACCESS";
                                                }
                                                else
                                                {
                                                    return "BASIC_ACCESS_TRAINING_REQUIRED";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            return "ACCESS_DENIED_NO_DEPARTMENT";
                                        }
                                    }
                                    else
                                    {
                                        if (accessLevel > 3)
                                        {
                                            return "MODERATE_ACCESS_NO_PERMISSIONS";
                                        }
                                        else
                                        {
                                            return "BASIC_ACCESS_ONLY";
                                        }
                                    }
                                }
                                else
                                {
                                    if (accessLevel > 2)
                                    {
                                        return "NON_ADMIN_MODERATE_ACCESS";
                                    }
                                    else
                                    {
                                        return "NON_ADMIN_BASIC_ACCESS";
                                    }
                                }
                            }
                            else
                            {
                                if (hasPermissions)
                                {
                                    if (accessLevel > 3)
                                    {
                                        return "NON_ADMIN_PREMIUM_HIGH_ACCESS";
                                    }
                                    else
                                    {
                                        return "NON_ADMIN_PREMIUM_STANDARD_ACCESS";
                                    }
                                }
                                else
                                {
                                    return "PREMIUM_USER_LIMITED_ACCESS";
                                }
                            }
                        }
                        else if (userType == "STANDARD")
                        {
                            if (isAdmin)
                            {
                                if (accessLevel > 4)
                                {
                                    return "ADMIN_STANDARD_HIGH_ACCESS";
                                }
                                else
                                {
                                    return "ADMIN_STANDARD_NORMAL_ACCESS";
                                }
                            }
                            else
                            {
                                if (hasPermissions)
                                {
                                    return "STANDARD_USER_WITH_PERMISSIONS";
                                }
                                else
                                {
                                    if (user.AccountAge > 365)
                                    {
                                        return "STANDARD_USER_VETERAN";
                                    }
                                    else
                                    {
                                        return "STANDARD_USER_BASIC";
                                    }
                                }
                            }
                        }
                        else if (userType == "TRIAL")
                        {
                            if (user.TrialDaysRemaining > 0)
                            {
                                if (user.TrialDaysRemaining > 7)
                                {
                                    return "TRIAL_ACTIVE_FULL_FEATURES";
                                }
                                else
                                {
                                    return "TRIAL_EXPIRING_SOON_LIMITED";
                                }
                            }
                            else
                            {
                                return "TRIAL_EXPIRED_UPGRADE_REQUIRED";
                            }
                        }
                        else
                        {
                            return "UNKNOWN_USER_TYPE_DEFAULT_ACCESS";
                        }
                    }
                    else
                    {
                        return "NULL_USER_TYPE_ACCESS_DENIED";
                    }
                }
                else
                {
                    if (user.SuspensionReason != null)
                    {
                        if (user.SuspensionReason == "TEMPORARY")
                        {
                            return "ACCOUNT_TEMPORARILY_SUSPENDED";
                        }
                        else
                        {
                            return "ACCOUNT_PERMANENTLY_SUSPENDED";
                        }
                    }
                    else
                    {
                        return "INACTIVE_ACCOUNT_REASON_UNKNOWN";
                    }
                }
            }
            else
            {
                if (user.Age >= 16)
                {
                    if (user.ParentalConsent)
                    {
                        return "MINOR_ACCESS_WITH_CONSENT";
                    }
                    else
                    {
                        return "MINOR_ACCESS_PARENTAL_CONSENT_REQUIRED";
                    }
                }
                else
                {
                    return "ACCESS_DENIED_TOO_YOUNG";
                }
            }
        }
        else
        {
            return "NULL_USER_ACCESS_DENIED";
        }
    }
}
