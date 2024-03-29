﻿namespace UserManagementService.Dtos
{
    public class Pagination
    {
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
        public int ItemsTotal { get; set; }
        public int PagesTotal { get; set; }
        public Pagination(int page, int itemsTotal, int itemsPerPage = 10)
        {
            Page = page;
            ItemsPerPage = itemsPerPage;
            ItemsTotal = itemsTotal;
            PagesTotal = ItemsTotal / itemsPerPage;
            if (ItemsTotal % itemsPerPage != 0)
                PagesTotal++;
        }
    }
}
