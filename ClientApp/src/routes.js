import Landing from './pages/Landing.vue'
import About from './pages/About.vue'
import Login from './pages/Login.vue'
import Logout from './pages/Logout.vue'
import Account from './pages/Account.vue'
import Dashboard from './pages/Dashboard.vue'
import Files from './pages/Files.vue'
import Counter from './pages/Counter.vue'
import Customers from './pages/Customers.vue'
import NotInRolePage from './pages/NotInRolePage.vue'
import AdminPage from './pages/admin/AdminPage.vue'
import Register from './pages/admin/Register.vue'
import UsersPage from './pages/admin/UsersPage.vue'
import GroupsPage from './pages/admin/GroupsPage.vue'
import GroupForm from './pages/admin/GroupForm.vue'
import FileCategoriesPage from './pages/admin/FileCategoriesPage.vue'
import FileCategoryForm from './pages/admin/FileCategoryForm.vue'
import ActivityLogsPage from './pages/admin/ActivityLogsPage.vue'
import GroupFiles from './pages/admin/GroupFiles.vue'
import FileCategoryFiles from './pages/admin/FileCategoryFiles.vue'
import UsersInGroup from './pages/admin/UsersInGroup.vue'
import AddUserToGroup from './pages/admin/AddUserToGroup.vue'

const main = [
  {
    path: '/',
    name: 'landing',
    component: Landing
  },
  {
    path: '/about',
    name: 'about',
    component: About
  },
  {
    path: '/login',
    name: 'login',
    component: Login
  },
  {
    path: '/register',
    name: 'register',
    component: Register,
    meta: { requiresAuth: true, requiresAdminRole : true }
  },
  {
    path: '/logout',
    name: 'logout',
    component: Logout
  },
  {
    path: '/d',
    name: 'dashboard',
    component: Dashboard,
    meta: { requiresAuth: true }
  },
  {
    path: '/u',
    name: 'account',
    component: Account,
    meta: { requiresAuth: true }
  },
  {
    path: '/f',
    name: 'files',
    component: Files,
    meta: { requiresAuth: true }
  },
  {
    path: '/counter',
    name: 'counter',
    component: Counter
  },
  {
    path: '/customers',
    name: 'customers',
    component: Customers,
    meta: { requiresAuth: true }
  },
  {
    path: '/admin',
    name: 'adminpage',
    component: AdminPage,
    meta: { requiresAuth: true, requiresAdminRole : true }
  },
  {
    path: '/notinrole',
    name: 'notinrole',
    component: NotInRolePage,
    meta: { requiresAuth: true }
  },
  {
    path: '/groups',
    name: 'groupspage',
    component: GroupsPage,
    meta: { requiresAuth: true, requiresAdminRole: true }
  },
  {
    path: '/groupedit/:id',
    name: 'groupedit',
    component: GroupForm,
    props: true, 
    meta: { requiresAuth: true, requiresAdminRole: true }
  },
  {
    path: '/newgroup',
    name: 'newgroup',
    component: GroupForm,
    meta: { requiresAuth: true, requiresAdminRole: true }
  },
  {
    path: '/users',
    name: 'userspage',
    component: UsersPage,
    meta: { requiresAuth: true, requiresAdminRole: true }
  },
  {
    path: '/filecategories',
    name: 'filecategories',
    component: FileCategoriesPage,
    meta: { requiresAuth: true, requiresAdminRole: true }
  },
  {
    path: '/filecategoryedit/:id',
    name: 'filecategoryedit',
    component: FileCategoryForm,
    props: true,
    meta: { requiresAuth: true, requiresAdminRole: true }
  },
  {
    path: '/newfilecategory',
    name: 'newfilecategory',
    component: FileCategoryForm,
    meta: { requiresAuth: true, requiresAdminRole: true }
  },
  {
    path: '/activitylogs',
    name: 'activitylogs',
    component: ActivityLogsPage,
    meta: { requiresAuth: true, requiresAdminRole: true }
  },
  {
    path: '/groupfiles/:id',
    name: 'groupfiles',
    component: GroupFiles,
    props: true,
    meta: { requiresAuth: true, requiresAdminRole: true }
  },
  {
    path: '/categoryfiles/:id',
    name: 'categoryfiles',
    component: FileCategoryFiles,
    props: true,
    meta: { requiresAuth: true, requiresAdminRole: true }
  },
  {
    path: '/usersingroup/:id',
    name: 'usersingroup',
    component: UsersInGroup,
    props: true,
    meta: { requiresAuth: true, requiresAdminRole: true }
  },
  {
    path: '/addusertogroup/:id',
    name: 'addusertogroup',
    component: AddUserToGroup,
    props: true,
    meta: { requiresAuth: true, requiresAdminRole: true }
  },
]

const error = [
]

export default [].concat(main, error)
