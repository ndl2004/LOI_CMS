import { Link, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import axiosClient from "../api/axiosClient";

function Header() {
    const [customer, setCustomer] = useState(null);
    const [cartCount, setCartCount] = useState(0);
    const [keyword, setKeyword] = useState("");
    const [categories, setCategories] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        loadData();
        loadCategories();

        window.addEventListener("storage", loadData);
        window.addEventListener("cartUpdated", loadData);
        window.addEventListener("customerUpdated", loadData);

        return () => {
            window.removeEventListener("storage", loadData);
            window.removeEventListener("cartUpdated", loadData);
            window.removeEventListener("customerUpdated", loadData);
        };
    }, []);

    useEffect(() => {
        const searchText = keyword.trim();

        if (!searchText) return;

        const timer = setTimeout(() => {
            navigate(
                `/search?keyword=${encodeURIComponent(searchText)}`
            );
        }, 350);

        return () => clearTimeout(timer);
    }, [keyword, navigate]);

    const loadData = () => {
        const customerData = JSON.parse(
            localStorage.getItem("customer")
        );

        const cart = JSON.parse(
            localStorage.getItem("cart")
        ) || [];

        const totalQuantity = cart.reduce(
            (sum, item) => sum + item.quantity,
            0
        );

        setCustomer(customerData);
        setCartCount(totalQuantity);
    };

    const loadCategories = () => {
        axiosClient
            .get("/CategoriesProducts")
            .then((res) => setCategories(res.data))
            .catch((err) => {
                console.error("Lỗi tải danh mục sản phẩm:", err);
                setCategories([]);
            });
    };

    const handleLogout = () => {
        localStorage.removeItem("customer");
        localStorage.removeItem("redirectAfterLogin");

        setCustomer(null);

        window.location.href = "/";
    };
    const handleSearch = () => {
        if (!keyword.trim()) return;

        navigate(
            `/search?keyword=${encodeURIComponent(keyword)}`
        );
    };
    const handleSearchChange = (e) => {
        setKeyword(e.target.value);
    };
    const handleKeyDown = (e) => {
        if (e.key === "Enter") {
            handleSearch();
        }
    };
    return (
        <div className="site-sticky-header">
            <div className="top-banner">
                ✨ LOI Cosmetics - Mỹ phẩm chính hãng, chăm sóc vẻ đẹp mỗi ngày ✨
            </div>

            <header className="header-main">
                <div className="container header-row">

                    <Link to="/" className="brand-logo">
                        <span className="brand-main">LOI-CMS</span>
                        <span className="brand-sub">
                            Beauty & Cosmetics
                        </span>
                    </Link>

                    <div className="search-box">
                        <input
                            value={keyword}
                            onChange={handleSearchChange}
                            onKeyDown={handleKeyDown}
                            placeholder="Tìm serum, kem chống nắng, sữa rửa mặt..."
                        />

                        <button onClick={handleSearch}>
                            🔍
                        </button>
                    </div>

                    <div className="header-actions">

                        {customer ? (
                            <div className="customer-dropdown">

                                <div className="header-action-item">
                                    <span className="action-icon">👤</span>

                                    <span>
                                        {customer.fullName}
                                    </span>
                                </div>

                                <div className="dropdown-menu-custom">

                                    <Link to="/profile">
                                        Hồ sơ cá nhân
                                    </Link>

                                    <Link to="/orders">
                                        Lịch sử mua hàng
                                    </Link>

                                    <button
                                        className="logout-btn"
                                        onClick={handleLogout}
                                    >
                                        Đăng xuất
                                    </button>

                                </div>
                            </div>
                        ) : (
                            <Link
                                to="/login"
                                className="header-action-item"
                            >
                                <span className="action-icon">👤</span>
                                <span>Đăng nhập</span>
                            </Link>
                        )}

                        <Link
                            to="/cart"
                            className="header-action-item cart-action"
                        >
                            <span className="action-icon">🛒</span>

                            <span>Giỏ hàng</span>

                            <span className="cart-badge">
                                {cartCount}
                            </span>
                        </Link>

                    </div>
                </div>
            </header>

            <nav className="navbar">
                <div className="container nav-row">
                   

                    <Link to="/">Trang chủ</Link>
                    <Link to="/shop">Sản phẩm</Link>
                    <Link to="/promotions">Khuyến mãi</Link>
                    <div className="nav-dropdown">
                        <Link to="/shop" className="nav-dropdown-label">
                            Danh mục
                        </Link>

                        <div className="nav-dropdown-menu">
                            {categories.length > 0 ? (
                                categories.map((cat) => (
                                    <Link
                                        key={cat.id}
                                        to={`/shop?category=${cat.id}`}
                                    >
                                        <span>{cat.name}</span>
                                        <small>{cat.description || "Xem sản phẩm"}</small>
                                    </Link>
                                ))
                            ) : (
                                <Link to="/shop">
                                    <span>Tất cả sản phẩm</span>
                                    <small>Xem danh sách sản phẩm</small>
                                </Link>
                            )}
                        </div>
                    </div>
                    <Link to="/blog">Cẩm nang làm đẹp</Link>
                </div>
            </nav>
        </div>
    );
}

export default Header;
