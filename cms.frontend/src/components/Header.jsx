import { Link } from "react-router-dom";
import { useEffect, useState } from "react";

function Header() {
    const [customer, setCustomer] = useState(null);
    const [cartCount, setCartCount] = useState(0);

    useEffect(() => {
        loadData();

        window.addEventListener("storage", loadData);
        window.addEventListener("cartUpdated", loadData);
        window.addEventListener("customerUpdated", loadData);

        return () => {
            window.removeEventListener("storage", loadData);
            window.removeEventListener("cartUpdated", loadData);
            window.removeEventListener("customerUpdated", loadData);
        };
    }, []);
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

    const handleLogout = () => {
        localStorage.removeItem("customer");

        setCustomer(null);

        window.location.href = "/";
    };

    return (
        <>
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
                        <input placeholder="Tìm serum, kem chống nắng, sữa rửa mặt..." />
                        <button>🔍</button>
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
                    <Link to="/brands">Thương hiệu</Link>
                    <Link to="/blog">Cẩm nang làm đẹp</Link>
                </div>
            </nav>
        </>
    );
}

export default Header;