namespace API.Common
{
    public class GlobalConstants
    {
        /// <summary>
        /// Error Constants
        /// </summary>
        public const string LOG_IN_ERROR = "Wrong password or email";
        public const string REGISTER_FAIL = "Registration failed.";
        public const string REGISTER_SUCCESS = "Librarian successfully registered.";
        public const string REGISTER_ERROR = "The email is already in use.";
        public const string INVALID_BOOK_ID_ERROR = "Book with the given id doesn't exist!";
        public const string BOOK_ALREADY_EXISTS_ERROR = "Book already exists!";
        public const string BOOK_NOT_FOUND_ERROR = "Book not found!";
        public const string CREATE_OR_UPDATE_BOOK_TITLE_EMPTY_ERROR = "Title can not be empty";
        public const string CREATE_OR_UPDATE_BOOK_TITLE_NULL_ERROR = "Title can not be null";
        public const string CREATE_OR_UPDATE_BOOK_IMAGE_EMPTY_ERROR = "Image can not be empty";
        public const string CREATE_OR_UPDATE_BOOK_IMAGE_NULL_ERROR = "Image can not be null";
        public const string CREATE_OR_UPDATE_BOOK_TITLE_MIN_LENGTH_ERROR = "Title must be more than 2 characters";
        public const string CREATE_OR_UPDATE_BOOK_TITLE_MAX_LENGTH_ERROR = "Title must be less than 120 characters";
        public const string CREATE_OR_UPDATE_BOOK_DESCRIPTION_EMPTY_ERROR = "Description can not be empty";
        public const string CREATE_OR_UPDATE_BOOK_DESCRIPTION_NULL_ERROR = "Description can not be null";
        public const string CREATE_OR_UPDATE_BOOK_DESCRIPTION_MAX_LENGTH_ERROR = "Description can not be more than 300 characters";
        public const string CREATE_OR_UPDATE_BOOK_QUANTITY_EMPTY_ERROR = "Quantity can not be empty";
        public const string CREATE_OR_UPDATE_BOOK_QUANTITY_NULL_ERROR = "Quantity can not be null";
        public const string CREATE_OR_UPDATE_BOOK_QUANTITY_MIN_LENGTH_ERROR = "Quantity must be more than 0";
        public const string CREATE_OR_UPDATE_BOOK_GENRE_EMPTY_ERROR = "Genre can not be empty";
        public const string CREATE_OR_UPDATE_BOOK_GENRE_NULL_ERROR = "Genre can not be null";
        public const string CREATE_OR_UPDATE_BOOK_AUTHOR_LAST_NAME_EMPTY_ERROR = "Author last name can not be empty";
        public const string CREATE_OR_UPDATE_BOOK_AUTHOR_LAST_NAME_NULL_ERROR = "Author last name can not be null";
        public const string CREATE_OR_UPDATE_BOOK_AUTHOR_LAST_NAME_MIN_LENGTH_ERROR = "Author last name must be more than 2 characters";
        public const string CREATE_OR_UPDATE_BOOK_AUTHOR_LAST_NAME_MAX_LENGTH_ERROR = "Author last name must be less than 50 characters";
        public const string NO_BOOKS_ON_PAGE_ERROR = "There are no books on this page!";
        public const string CREATE_OR_EDIT_BOOK_NO_IMAGE = "Image Required";
        public const string UPLOAD_IMAGE_ERROR = "Upload image failed";
        public const string GET_IMAGE_ERROR = "Download image failed";
        public const string DELETE_IMAGE_ERROR = "Delete image failed";
        public const string NO_NEW_REQUEST_MESSAGE = "No new requests";
        public const string OUT_OF_STOCK_MESSAGE = "Book out of stock";
        public const string REQUEST_NOT_FOUND_MESSAGE = "Request not fount";
        public const string DUPLICATE_REQUEST_MESSAGE = "Request already approved";
        public const string NO_BOOKS_TO_RETURN_TWO_WEEKS_MESSAGE = "No books fount to be returned in two week";
        public const string NO_BOOKS_DELAYED_RETURN = "No books found to be delayed";

        public const string USER_NOT_FOUND = "The user you are looking for was not found";
        public const string REQUEST_DUPLICATE_ERROR = "This request already exists";
        /// <summary>
        /// Validation Constants
        /// </summary>
        public const int MIN_PASSWORD_LENGTH = 10;
        public const string PASSWORD_ALLOWED_CHARACTERS = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{10,44}$";
        public const string EMAIL_VALIDATION_REGEX = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@"  + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z";
        public const int MIN_LENGTH_FIRST_OR_LASTNAME = 2;
        public const int MAX_LENGTH_FIRST_OR_LASTNAME = 50;
        public const int MIN_LENGTH_TITLE = 2;
        public const int MAX_LENGTH_TITLE = 120;
        public const int MAX_LENGTH_DESCRIPTION = 300;
        public const int MIN_VALUE_QUANTITY = 0;
        public const int RENT_BOOK_PERIOD = 30;
        public const string SENDGRID_API_KEY = "Add APY Key Here";
        public const string SEND_EMAIL_FAIL = "Send email failed.";
        public const string CHANGE_PASSWORD_FAIL = "Password change failed";

    }
}
