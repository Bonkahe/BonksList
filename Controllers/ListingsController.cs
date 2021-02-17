﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BonksList.Data;
using BonksList.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.Globalization;

namespace BonksList.Controllers
{
    public class ListingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ListingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Listings
        public async Task<IActionResult> Index()
        {

            SearchListingsModel newListingModel = new SearchListingsModel
            {
                filters = GetFilters(),
                currentFilter = new SelectListItem() { Text = "None", Value = string.Empty },
                listings = await _context.Listing.ToListAsync()
            };
            return View(newListingModel);
        }

        // GET: Listings/ShowSearchForm
        public IActionResult ShowSearchForm()
        {
            return View();
        }

        // POST: Listings/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(string SearchPhrase)
        {
            SearchListingsModel newListingModel = new SearchListingsModel
            {
                filters = GetFilters(),
                currentFilter = new SelectListItem() { Text = "None", Value = string.Empty },
                listings = await _context.Listing.Where(j => j.description.Contains(SearchPhrase)).ToListAsync()
            };

            return View("Index", newListingModel);
        }

        public async Task<IActionResult> UpdateFilter(SelectListItem selectedFilter)
        {
            if (selectedFilter.Text != "None")
            {
                // selected a filter
                SearchListingsModel newListingModel = new SearchListingsModel
                {
                    filters = GetFilters(),
                    currentFilter = selectedFilter,
                    listings = await _context.Listing.ToListAsync()
                };
                return View("Index", newListingModel);
            }
            else
            {
                SearchListingsModel newListingModel = new SearchListingsModel
                {
                    filters = GetFilters(),
                    currentFilter = selectedFilter,
                    listings = await _context.Listing.Where(j => j.listingType == selectedFilter.Value).ToListAsync()
                };
                return View("Index", newListingModel);
            }
        }

        // GET: Listings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listing = await _context.Listing
                .FirstOrDefaultAsync(m => m.Id == id);
            if (listing == null)
            {
                return NotFound();
            }

            return View(listing);
        }

        // GET: Listings/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Listings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,imageUrl,description,price,listingType")] Listing listing)
        {
            if (ModelState.IsValid)
            {
                listing.accountId = User.Identity.Name;

                if (listing.imageUrl != null)
                {
                    if (!IsImageUrl(listing.imageUrl))
                    {
                        listing.imageUrl = null;
                    }
                }

                _context.Add(listing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(listing);
        }

        // GET: Listings/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listing = await _context.Listing.FindAsync(id);
            if (listing == null)
            {
                return NotFound();
            }
            return View(listing);
        }

        // POST: Listings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,imageUrl,description,price")] Listing listing)
        {
            if (id != listing.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (listing == null)
                {
                    return NotFound();
                }

                var prevlisting = await _context.Listing.AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (prevlisting.accountId != User.Identity.Name)
                {
                    return NotFound();
                }
                else
                {
                    listing.accountId = User.Identity.Name;
                }

                if (listing.imageUrl != null)
                {
                    if (!IsImageUrl(listing.imageUrl))
                    {
                        listing.imageUrl = null;
                    }
                }

                try
                {
                    _context.Update(listing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListingExists(listing.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(listing);
        }

        // GET: Listings/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listing = await _context.Listing
                .FirstOrDefaultAsync(m => m.Id == id);
            if (listing == null)
            {
                return NotFound();
            }

            return View(listing);
        }

        // POST: Listings/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var listing = await _context.Listing.FindAsync(id);

            if (listing.accountId != User.Identity.Name)
            {
                return NotFound();
            }

            _context.Listing.Remove(listing);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ListingExists(int id)
        {
            return _context.Listing.Any(e => e.Id == id);
        }
        /// <summary>
        /// Does exactly what it sounds like, be careful using this.
        /// </summary>
        private void EmptyListingTable()
        {
            foreach (var id in _context.Listing.Select(e => e.Id))
            {
                var entity = new Listing { Id = id };
                _context.Listing.Attach(entity);
                _context.Listing.Remove(entity);
            }
            _context.SaveChanges();
        }

        /// <summary>
        /// Check if the url given is an image url, actually checks the server for this, so it does have overhead.
        /// </summary>
        /// <param name="URL">The url to be checked.</param>
        /// <returns>True if the url is viable as an image.</returns>
        bool IsImageUrl(string URL)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(URL, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (!result)
            {
                return false;
            }

            var req = (HttpWebRequest)HttpWebRequest.Create(uriResult);
            req.Method = "HEAD";
            using (var resp = req.GetResponse())
            {
                return resp.ContentType.ToLower(CultureInfo.InvariantCulture)
                           .StartsWith("image/");
            }
        }

        /// <summary>
        /// Populates filter List
        /// </summary>
        /// <returns>list of filter options</returns>
        private IEnumerable<SelectListItem> GetFilters()
        {
            return new SelectListItem[]
{
                new SelectListItem() { Text = "None", Value = string.Empty },
                new SelectListItem() { Text = "For Sale", Value = "forsale" },
                new SelectListItem() { Text = "For Free", Value = "forfree" },
                new SelectListItem() { Text = "Service", Value = "service" }
            };
        }
    }
}
