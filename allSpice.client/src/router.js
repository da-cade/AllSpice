import { createRouter, createWebHashHistory } from 'vue-router'
import { authGuard } from '@bcwdev/auth0provider-client'

function loadPage(page) {
  return () => import(`./pages/${page}.vue`)
}
function loadComponent(component) {
  return () => import(`./components/${component}.vue`)
}


const routes = [
  {
    path: '/',
    name: 'Home',
    component: loadPage('HomePage'),
    // REVIEW another option could have been to use a query paramter and a watchEffect on whether or not that paramater was applied.
    // this still would probably need to be watched on each Recipe.
    children: [
      {
        path: '/recipes',
        name: 'AllRecipes',
        component: loadComponent('RecipesFeed'),
        meta: {
          showModal: false
        }
      },
      {
        path: '/recipes/:recipeId/details',
        name: 'RecipeDetails',
        component: loadComponent('RecipeDetails'),
        props: true,
        meta: {
          showModal: true
        }
      },
    ]
  },

  {
    path: '/newRecipe',
    name: 'New Recipe',
    component: loadPage('RecipeForm')
  },
  {
    path: '/account',
    name: 'Account',
    component: loadPage('AccountPage'),
    beforeEnter: authGuard
  }
]

export const router = createRouter({
  linkActiveClass: 'router-link-active',
  linkExactActiveClass: 'router-link-exact-active',
  history: createWebHashHistory(),
  routes
})
