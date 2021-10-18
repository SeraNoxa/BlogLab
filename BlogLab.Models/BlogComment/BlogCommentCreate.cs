using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlogLab.Models.BlogComment
{
    public class BlogCommentCreate
    {
        public int BlogCommentId { get; set; }

        public int? ParentBlogCommentId { get; set; }

        public int BlogId { get; set; }

        [Required(ErrorMessage = "Content is required")]
        [MinLength(10, ErrorMessage = "Must be atleast 10 characters")]
        [MaxLength(300, ErrorMessage = "Must be between 10-300 characters, you need to shorten your message")]
        public string Content { get; set; }

    }
}
