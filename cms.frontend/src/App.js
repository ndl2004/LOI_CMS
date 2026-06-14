import { Routes, Route } from "react-router-dom";
import "./App.css";

import Header from "./components/Header";
import Footer from "./components/Footer";

import Home from "./pages/Home";
import Shop from "./pages/Shop";
import ProductDetail from "./pages/ProductDetail";
import Cart from "./pages/Cart";
import Checkout from "./pages/Checkout";
import Login from "./pages/Login";
import Register from "./pages/Register";
import OrderHistory from "./pages/OrderHistory";
import BlogDetail from "./pages/BlogDetail";
import Blog from "./pages/Blog";
import Search from "./pages/Search";
import ForgotPassword from "./pages/ForgotPassword";
function App() {
    return (
        <>
            <Header />

            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/shop" element={<Shop />} />
                <Route path="/product/:id" element={<ProductDetail />} />
                <Route path="/cart" element={<Cart />} />
                <Route path="/checkout" element={<Checkout />} />
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route path="/orders" element={<OrderHistory />} />
                <Route path="/blog/:id" element={<BlogDetail />} />
                <Route path="/blog" element={<Blog />} />
                <Route path="/search" element={<Search />} />
                <Route path="/forgot-password" element={<ForgotPassword />} />

            </Routes>

            <Footer />
        </>
    );
}

export default App;