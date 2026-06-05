import { Link } from "react-router-dom";

function Footer() {
    return (
        <footer className="beauty-footer">
            <div className="container footer-grid">
                <div className="footer-brand">
                    <h2>
                        LOI<span>Cosmetics</span>
                    </h2>
                    <p>
                        Website bán mỹ phẩm chính hãng, chăm sóc da và làm đẹp mỗi ngày.
                        Kết nối ReactJS với ASP.NET Core Web API.
                    </p>
                    <div className="footer-social">
                        <span>f</span>
                        <span>ig</span>
                        <span>tt</span>
                    </div>
                </div>

                <div className="footer-col">
                    <h4>Danh mục</h4>
                    <Link to="/shop">Sữa rửa mặt</Link>
                    <Link to="/shop">Kem chống nắng</Link>
                    <Link to="/shop">Serum</Link>
                    <Link to="/shop">Kem dưỡng</Link>
                </div>

                <div className="footer-col">
                    <h4>Hỗ trợ</h4>
                    <Link to="/shop">Sản phẩm</Link>
                    <Link to="/cart">Giỏ hàng</Link>
                    <Link to="/checkout">Thanh toán</Link>
                    <Link to="/">Cẩm nang làm đẹp</Link>
                </div>

                <div className="footer-col">
                    <h4>Liên hệ</h4>
                    <p>📍 TP. Hồ Chí Minh</p>
                    <p>📞 0909 123 456</p>
                    <p>✉️ loicosmetics@gmail.com</p>
                    <p>⏰ 8:00 - 22:00</p>
                </div>
            </div>

            <div className="footer-bottom">
                © 2026 LOI Cosmetics. Buổi 06 - Web API cho Frontend ReactJS.
            </div>
        </footer>
    );
}

export default Footer;