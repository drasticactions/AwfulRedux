﻿namespace Imgur.API.Enums
{
    /// <summary>
    ///     The order that the gallery should be sorted by.
    /// </summary>
    public enum SubredditGallerySortOrder
    {
        /// <summary>
        ///     Sort the gallery by the most recent item first.
        /// </summary>
        Time,

        /// <summary>
        ///     Sort the gallery by the most top rated item in a period first.
        /// </summary>
        Top
    }
}