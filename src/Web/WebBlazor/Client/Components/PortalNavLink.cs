using System;
using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Routing;

namespace WebBlazor.Client.Components
{
    public class PortalNavLink : NavLink, IDisposable
    {
        private const string DefaultActiveClass = "active";
        private string? _class;
        private string? _hrefAbsolute;

        private bool _isActive;

        [Inject]
        private NavigationManager NavigationManger { get; set; } = default!;

        /// <inheritdoc />
        public new void Dispose()
        {
            GC.SuppressFinalize(this);

            // To avoid leaking memory, it's important to detach any event handlers in Dispose()
            NavigationManger.LocationChanged -= OnLocationChanged;
        }

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            // We'll consider re-rendering on each location change
            NavigationManger.LocationChanged += OnLocationChanged;
        }

        /// <inheritdoc />
        protected override void OnParametersSet()
        {
            // Update computed state
            var href = (string?) null;
            if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("href", out var obj))
            {
                href = Convert.ToString(obj, CultureInfo.InvariantCulture);
            }

            _hrefAbsolute = href == null ? null : NavigationManger.ToAbsoluteUri(href).AbsoluteUri;
            _isActive = ShouldMatch(NavigationManger.Uri);

            _class = null;
            if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("class", out obj))
            {
                _class = Convert.ToString(obj, CultureInfo.InvariantCulture);
            }

            UpdateCssClass();
        }

        private void UpdateCssClass()
        {
            CssClass = _isActive ? CombineWithSpace(_class, ActiveClass ?? DefaultActiveClass) : _class;
        }

        private void OnLocationChanged(object? sender, LocationChangedEventArgs args)
        {
            // We could just re-render always, but for this component we know the
            // only relevant state change is to the _isActive property.
            var shouldBeActiveNow = ShouldMatch(args.Location);
            if (shouldBeActiveNow != _isActive)
            {
                _isActive = shouldBeActiveNow;
                UpdateCssClass();
                StateHasChanged();
            }
        }

        private bool ShouldMatch(string currentUriAbsolute)
        {
            if (_hrefAbsolute == null)
            {
                return false;
            }

            if (EqualsHrefExactlyOrIfTrailingSlashAdded(currentUriAbsolute))
            {
                return true;
            }

            if (Match == NavLinkMatch.Prefix
                && IsStrictlyPrefixWithSeparator(currentUriAbsolute, _hrefAbsolute))
            {
                return true;
            }

            return false;
        }

        private bool EqualsHrefExactlyOrIfTrailingSlashAdded(string currentUriAbsolute)
        {
            Debug.Assert(_hrefAbsolute != null);

            if (string.Equals(currentUriAbsolute, _hrefAbsolute, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (currentUriAbsolute.Length == _hrefAbsolute.Length - 1)
            {
                // Special case: highlight links to http://host/path/ even if you're
                // at http://host/path (with no trailing slash)
                //
                // This is because the router accepts an absolute URI value of "same
                // as base URI but without trailing slash" as equivalent to "base URI",
                // which in turn is because it's common for servers to return the same page
                // for http://host/vdir as they do for host://host/vdir/ as it's no
                // good to display a blank page in that case.
                if (_hrefAbsolute[_hrefAbsolute.Length - 1] == '/'
                    && _hrefAbsolute.StartsWith(currentUriAbsolute, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc />
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "li");
            builder.AddAttribute(1, "class", CssClass);
            builder.OpenElement(2, "a");

            builder.AddMultipleAttributes(3, AdditionalAttributes);
            builder.AddAttribute(4, "class", "nav-link");
            builder.AddContent(5, ChildContent);

            builder.CloseElement();
            builder.CloseElement();
        }

        private static string? CombineWithSpace(string? str1, string str2)
        {
            return str1 == null ? str2 : $"{str1} {str2}";
        }

        private static bool IsStrictlyPrefixWithSeparator(string value, string prefix)
        {
            var prefixLength = prefix.Length;
            if (value.Length > prefixLength)
            {
                return value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
                       && (
                           // Only match when there's a separator character either at the end of the
                           // prefix or right after it.
                           // Example: "/abc" is treated as a prefix of "/abc/def" but not "/abcdef"
                           // Example: "/abc/" is treated as a prefix of "/abc/def" but not "/abcdef"
                           prefixLength == 0
                           || !char.IsLetterOrDigit(prefix[prefixLength - 1])
                           || !char.IsLetterOrDigit(value[prefixLength])
                       );
            }

            return false;
        }
    }
}